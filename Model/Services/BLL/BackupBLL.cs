using System;
using DomainModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAL.Implementations;
using Services.DomainModel.Security.Composite;

namespace Services.BLL
{
    /// <summary>
    /// Capa de lógica de negocio para gestión de Backups.
    /// Implementa validaciones y reglas de negocio para operaciones de backup/restore.
    /// </summary>
    public class BackupBLL
    {
        private readonly BackupRepository _backupRepository;
        private readonly string _rutaBackupsPorDefecto;

        public BackupBLL()
        {
            _backupRepository = BackupRepository.Current;

            // Ruta por defecto: C:\BackupsBiblioteca (evita problemas con OneDrive)
            _rutaBackupsPorDefecto = @"C:\BackupsBiblioteca";

            // Asegurar que existe el directorio
            if (!Directory.Exists(_rutaBackupsPorDefecto))
            {
                Directory.CreateDirectory(_rutaBackupsPorDefecto);
            }
        }

        #region Creación de Backups

        /// <summary>
        /// Crea un backup completo de una base de datos
        /// </summary>
        public Backup CrearBackup(string nombreBaseDatos, Usuario usuario, string descripcion = null, string rutaPersonalizada = null)
        {
            // Validaciones
            ValidarNombreBaseDatos(nombreBaseDatos);

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Debe proporcionar un usuario válido");

            // Generar nombre de archivo con timestamp
            string nombreArchivo = GenerarNombreArchivo(nombreBaseDatos, "Full");
            string rutaArchivo = string.IsNullOrEmpty(rutaPersonalizada)
                ? Path.Combine(_rutaBackupsPorDefecto, nombreArchivo)
                : Path.Combine(rutaPersonalizada, nombreArchivo);

            // Validar ruta
            ValidarRutaBackup(rutaArchivo);

            // Crear registro en catálogo
            Backup backup = new Backup
            {
                FechaCreacion = DateTime.Now,
                NombreBaseDatos = nombreBaseDatos,
                RutaArchivo = rutaArchivo,
                TipoBackup = "Full",
                Estado = "En Proceso",
                IdUsuarioCreacion = usuario.IdUsuario,
                Descripcion = descripcion
            };

            try
            {
                // Registrar backup en catálogo
                int idBackup = _backupRepository.RegistrarBackup(backup);
                backup.IdBackup = idBackup;

                // Ejecutar backup físico
                _backupRepository.EjecutarBackup(nombreBaseDatos, rutaArchivo, "Full");

                // Obtener tamaño del archivo
                decimal tamañoMB = _backupRepository.ObtenerTamañoArchivo(rutaArchivo);

                // Actualizar estado a exitoso
                _backupRepository.ActualizarEstadoBackup(idBackup, "Exitoso", tamañoMB, null);
                backup.Estado = "Exitoso";
                backup.TamañoMB = tamañoMB;

                // Registrar en bitácora de seguridad
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Backup",
                    "Backup Creado",
                    $"Backup exitoso de '{nombreBaseDatos}'. Tamaño: {tamañoMB:N2} MB. Archivo: {nombreArchivo}",
                    "INFO"
                );

                return backup;
            }
            catch (Exception ex)
            {
                // Actualizar estado a fallido
                if (backup.IdBackup > 0)
                {
                    _backupRepository.ActualizarEstadoBackup(backup.IdBackup, "Fallido", null, ex.Message);
                }

                // Registrar error en bitácora
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Backup",
                    "Error en Backup",
                    $"Error al crear backup de '{nombreBaseDatos}': {ex.Message}",
                    "ERROR"
                );

                throw new Exception($"Error al crear backup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un backup diferencial de una base de datos
        /// </summary>
        public Backup CrearBackupDiferencial(string nombreBaseDatos, Usuario usuario, string descripcion = null, string rutaPersonalizada = null)
        {
            // Validar que existe al menos un backup completo
            var backupsExistentes = _backupRepository.ObtenerPorBaseDatos(nombreBaseDatos);
            if (!backupsExistentes.Any(b => b.TipoBackup == "Full" && b.Estado == "Exitoso"))
            {
                throw new Exception("Debe existir al menos un backup completo antes de crear un backup diferencial");
            }

            ValidarNombreBaseDatos(nombreBaseDatos);

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Debe proporcionar un usuario válido");

            string nombreArchivo = GenerarNombreArchivo(nombreBaseDatos, "Differential");
            string rutaArchivo = string.IsNullOrEmpty(rutaPersonalizada)
                ? Path.Combine(_rutaBackupsPorDefecto, nombreArchivo)
                : Path.Combine(rutaPersonalizada, nombreArchivo);

            ValidarRutaBackup(rutaArchivo);

            Backup backup = new Backup
            {
                FechaCreacion = DateTime.Now,
                NombreBaseDatos = nombreBaseDatos,
                RutaArchivo = rutaArchivo,
                TipoBackup = "Differential",
                Estado = "En Proceso",
                IdUsuarioCreacion = usuario.IdUsuario,
                Descripcion = descripcion
            };

            try
            {
                int idBackup = _backupRepository.RegistrarBackup(backup);
                backup.IdBackup = idBackup;

                _backupRepository.EjecutarBackup(nombreBaseDatos, rutaArchivo, "Differential");

                decimal tamañoMB = _backupRepository.ObtenerTamañoArchivo(rutaArchivo);

                _backupRepository.ActualizarEstadoBackup(idBackup, "Exitoso", tamañoMB, null);
                backup.Estado = "Exitoso";
                backup.TamañoMB = tamañoMB;

                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Backup",
                    "Backup Diferencial Creado",
                    $"Backup diferencial exitoso de '{nombreBaseDatos}'. Tamaño: {tamañoMB:N2} MB",
                    "INFO"
                );

                return backup;
            }
            catch (Exception ex)
            {
                if (backup.IdBackup > 0)
                {
                    _backupRepository.ActualizarEstadoBackup(backup.IdBackup, "Fallido", null, ex.Message);
                }

                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Backup",
                    "Error en Backup Diferencial",
                    $"Error al crear backup diferencial de '{nombreBaseDatos}': {ex.Message}",
                    "ERROR"
                );

                throw new Exception($"Error al crear backup diferencial: {ex.Message}", ex);
            }
        }

        #endregion

        #region Restauración

        /// <summary>
        /// Restaura una base de datos desde un backup
        /// ADVERTENCIA: Esta operación sobrescribirá la base de datos actual
        /// </summary>
        public void RestaurarBackup(int idBackup, Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Debe proporcionar un usuario válido");

            // Obtener backup del catálogo
            Backup backup = _backupRepository.ObtenerPorId(idBackup);
            if (backup == null)
                throw new Exception("El backup especificado no existe");

            if (backup.Estado != "Exitoso")
                throw new Exception("Solo se pueden restaurar backups con estado 'Exitoso'");

            // Validar que el archivo existe
            if (!File.Exists(backup.RutaArchivo))
                throw new FileNotFoundException($"El archivo de backup no existe: {backup.RutaArchivo}");

            try
            {
                // Registrar inicio de restore
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Restore",
                    "Iniciando Restore",
                    $"Iniciando restauración de '{backup.NombreBaseDatos}' desde archivo: {backup.NombreArchivo}",
                    "WARNING"
                );

                // Ejecutar restore
                _backupRepository.EjecutarRestore(backup.NombreBaseDatos, backup.RutaArchivo);

                // Registrar éxito
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Restore",
                    "Restore Exitoso",
                    $"Base de datos '{backup.NombreBaseDatos}' restaurada exitosamente desde: {backup.NombreArchivo}",
                    "INFO"
                );
            }
            catch (Exception ex)
            {
                // Registrar error
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Restore",
                    "Error en Restore",
                    $"Error al restaurar '{backup.NombreBaseDatos}': {ex.Message}",
                    "CRITICAL"
                );

                throw new Exception($"Error al restaurar backup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Restaura desde un archivo de backup externo (no registrado en el catálogo)
        /// </summary>
        public void RestaurarDesdeArchivo(string nombreBaseDatos, string rutaArchivo, Usuario usuario)
        {
            ValidarNombreBaseDatos(nombreBaseDatos);

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Debe proporcionar un usuario válido");

            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException($"El archivo de backup no existe: {rutaArchivo}");

            try
            {
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Restore",
                    "Iniciando Restore Externo",
                    $"Iniciando restauración de '{nombreBaseDatos}' desde archivo externo: {Path.GetFileName(rutaArchivo)}",
                    "WARNING"
                );

                _backupRepository.EjecutarRestore(nombreBaseDatos, rutaArchivo);

                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Restore",
                    "Restore Externo Exitoso",
                    $"Base de datos '{nombreBaseDatos}' restaurada exitosamente desde archivo externo",
                    "INFO"
                );
            }
            catch (Exception ex)
            {
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Restore",
                    "Error en Restore Externo",
                    $"Error al restaurar '{nombreBaseDatos}': {ex.Message}",
                    "CRITICAL"
                );

                throw new Exception($"Error al restaurar backup: {ex.Message}", ex);
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Obtiene todos los backups registrados
        /// </summary>
        public List<Backup> ObtenerTodosLosBackups()
        {
            return _backupRepository.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene backups de una base de datos específica
        /// </summary>
        public List<Backup> ObtenerBackupsPorBaseDatos(string nombreBaseDatos)
        {
            ValidarNombreBaseDatos(nombreBaseDatos);
            return _backupRepository.ObtenerPorBaseDatos(nombreBaseDatos);
        }

        /// <summary>
        /// Obtiene un backup por ID
        /// </summary>
        public Backup ObtenerBackupPorId(int idBackup)
        {
            return _backupRepository.ObtenerPorId(idBackup);
        }

        #endregion

        #region Gestión de Archivos

        /// <summary>
        /// Elimina un backup (archivo físico y registro)
        /// </summary>
        public void EliminarBackup(int idBackup, Usuario usuario, bool eliminarArchivo = true)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "Debe proporcionar un usuario válido");

            Backup backup = _backupRepository.ObtenerPorId(idBackup);
            if (backup == null)
                throw new Exception("El backup especificado no existe");

            try
            {
                // Eliminar archivo físico si se solicita
                if (eliminarArchivo && File.Exists(backup.RutaArchivo))
                {
                    File.Delete(backup.RutaArchivo);
                }

                // Eliminar registro del catálogo
                _backupRepository.EliminarRegistro(idBackup);

                // Registrar en bitácora
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Backup",
                    "Backup Eliminado",
                    $"Backup eliminado: {backup.NombreArchivo} (Base de datos: {backup.NombreBaseDatos})",
                    "INFO"
                );
            }
            catch (Exception ex)
            {
                BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                    usuario.IdUsuario,
                    usuario.Nombre,
                    "Backup",
                    "Error al Eliminar Backup",
                    $"Error al eliminar backup {backup.NombreArchivo}: {ex.Message}",
                    "ERROR"
                );

                throw new Exception($"Error al eliminar backup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica el espacio disponible en el disco
        /// </summary>
        public long ObtenerEspacioDisponibleMB(string ruta = null)
        {
            try
            {
                string rutaVerificar = ruta ?? _rutaBackupsPorDefecto;
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(rutaVerificar));
                return drive.AvailableFreeSpace / (1024 * 1024); // Convertir a MB
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Validaciones Privadas

        private void ValidarNombreBaseDatos(string nombreBaseDatos)
        {
            if (string.IsNullOrWhiteSpace(nombreBaseDatos))
                throw new ArgumentException("El nombre de la base de datos no puede estar vacío");

            // Validar que sea una de las bases de datos conocidas
            string[] basesPermitidas = { "SeguridadBiblioteca", "NegocioBiblioteca" };
            if (!basesPermitidas.Contains(nombreBaseDatos))
                throw new ArgumentException($"Base de datos no permitida. Solo se permiten: {string.Join(", ", basesPermitidas)}");
        }

        private void ValidarRutaBackup(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta del archivo no puede estar vacía");

            // Validar extensión
            string extension = Path.GetExtension(rutaArchivo);
            if (extension.ToLower() != ".bak")
                throw new ArgumentException("El archivo debe tener extensión .bak");

            // Validar que el directorio existe o se puede crear
            string directorio = Path.GetDirectoryName(rutaArchivo);
            if (!Directory.Exists(directorio))
            {
                try
                {
                    Directory.CreateDirectory(directorio);
                }
                catch (Exception ex)
                {
                    throw new Exception($"No se puede crear el directorio: {ex.Message}", ex);
                }
            }

            // Validar que no exista ya el archivo
            if (File.Exists(rutaArchivo))
                throw new Exception("Ya existe un archivo con ese nombre. Elija otro nombre o elimine el archivo existente.");
        }

        private string GenerarNombreArchivo(string nombreBaseDatos, string tipoBackup)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string tipo = tipoBackup == "Full" ? "Full" : "Diff";
            return $"{nombreBaseDatos}_{tipo}_{timestamp}.bak";
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene la ruta por defecto para almacenar backups
        /// </summary>
        public string RutaBackupsPorDefecto
        {
            get { return _rutaBackupsPorDefecto; }
        }

        #endregion
    }
}
