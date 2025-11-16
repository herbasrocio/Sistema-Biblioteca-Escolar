using System;
using System.Collections.Generic;
using System.Linq;
using Services.DomainModel.Security.Composite;
using Services.DomainModel.Exceptions;
using Services.Services;

namespace Services.BLL
{
    public static class UsuarioBLL
    {
        /// <summary>
        /// Obtiene todos los usuarios del sistema
        /// </summary>
        public static IEnumerable<Usuario> ObtenerTodosLosUsuarios()
        {
            try
            {
                return DAL.Implementations.UsuarioRepository.Current.SelectAll();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener los usuarios", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por ID
        /// </summary>
        public static Usuario ObtenerUsuarioPorId(Guid id)
        {
            try
            {
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(id);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {id}");
                }
                return usuario;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un usuario por nombre
        /// </summary>
        public static Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            try
            {
                ValidarCampoRequerido(nombre, "Nombre de usuario");

                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException(nombre);
                }
                return usuario;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña</param>
        /// <param name="idFamiliaRol">ID de la Familia que representa el rol</param>
        public static void CrearUsuario(string nombre, string email, string password, Guid idFamiliaRol)
        {
            try
            {
                // Validaciones
                ValidarCampoRequerido(nombre, "Nombre de usuario");
                ValidarCampoRequerido(password, "Contraseña");
                ValidarLongitudMinima(password, "Contraseña", 6);

                // Verificar que la familia de rol exista y sea válida
                var familiaRol = DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamiliaRol);
                if (familiaRol == null)
                {
                    throw new ValidacionException("El rol seleccionado no existe");
                }
                if (!familiaRol.EsRol)
                {
                    throw new ValidacionException("La familia seleccionada no representa un rol válido");
                }

                // Verificar que el usuario no exista
                var usuarioExistente = DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);
                if (usuarioExistente != null)
                {
                    throw new ValidacionException($"Ya existe un usuario con el nombre '{nombre}'");
                }

                // Crear el usuario
                var nuevoUsuario = new Usuario
                {
                    IdUsuario = Guid.NewGuid(),
                    Nombre = nombre,
                    Email = email,
                    Password = password,
                    Clave = CryptographyService.HashPassword(password),
                    Activo = true
                };

                DAL.Implementations.UsuarioRepository.Current.Insert(nuevoUsuario);

                // Asignar el rol (familia) al usuario
                AsignarFamilia(nuevoUsuario.IdUsuario, idFamiliaRol);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="idUsuario">ID del usuario a actualizar</param>
        /// <param name="nombre">Nuevo nombre</param>
        /// <param name="email">Nuevo email</param>
        /// <param name="password">Nueva contraseña (opcional, dejar vacío para no cambiar)</param>
        /// <param name="idFamiliaRol">ID de la nueva Familia de rol (opcional, usar Guid.Empty para no cambiar)</param>
        public static void ActualizarUsuario(Guid idUsuario, string nombre, string email, string password, Guid? idFamiliaRol)
        {
            try
            {
                // Validaciones
                ValidarCampoRequerido(nombre, "Nombre de usuario");

                if (!string.IsNullOrWhiteSpace(password))
                {
                    ValidarLongitudMinima(password, "Contraseña", 6);
                }

                // Obtener usuario actual
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                // Verificar que el nuevo nombre no esté en uso por otro usuario
                if (usuario.Nombre != nombre)
                {
                    var usuarioConMismoNombre = DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);
                    if (usuarioConMismoNombre != null && usuarioConMismoNombre.IdUsuario != idUsuario)
                    {
                        throw new ValidacionException($"Ya existe otro usuario con el nombre '{nombre}'");
                    }
                }

                // Actualizar datos
                usuario.Nombre = nombre;
                usuario.Email = email;

                // Solo actualizar contraseña si se proporcionó una nueva
                if (!string.IsNullOrWhiteSpace(password))
                {
                    usuario.Password = password;
                    usuario.Clave = CryptographyService.HashPassword(password);
                }

                DAL.Implementations.UsuarioRepository.Current.Update(usuario);

                // Actualizar rol si se proporcionó uno nuevo
                if (idFamiliaRol.HasValue && idFamiliaRol.Value != Guid.Empty)
                {
                    CambiarRol(idUsuario, idFamiliaRol.Value);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Cambia el rol de un usuario (quita el rol anterior y asigna uno nuevo)
        /// </summary>
        public static void CambiarRol(Guid idUsuario, Guid idNuevaFamiliaRol)
        {
            try
            {
                // Verificar que la nueva familia de rol exista y sea válida
                var nuevaFamiliaRol = DAL.Implementations.FamiliaRepository.Current.SelectOne(idNuevaFamiliaRol);
                if (nuevaFamiliaRol == null)
                {
                    throw new ValidacionException("El rol seleccionado no existe");
                }
                if (!nuevaFamiliaRol.EsRol)
                {
                    throw new ValidacionException("La familia seleccionada no representa un rol válido");
                }

                // Quitar rol anterior (quitar todas las familias de rol)
                var familiasUsuario = ObtenerFamiliasDelUsuario(idUsuario);
                foreach (var familia in familiasUsuario.Where(f => f.EsRol))
                {
                    QuitarFamilia(idUsuario, familia.IdComponent);
                }

                // Asignar nuevo rol
                AsignarFamilia(idUsuario, idNuevaFamiliaRol);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al cambiar el rol del usuario", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario del sistema
        /// </summary>
        public static void EliminarUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                DAL.Implementations.UsuarioRepository.Current.Delete(idUsuario);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de Familias que representan roles disponibles
        /// Familias con nombres "ROL_Administrador", "ROL_Veterinario", etc.
        /// </summary>
        public static IEnumerable<Familia> ObtenerRolesDisponibles()
        {
            try
            {
                var todasFamilias = DAL.Implementations.FamiliaRepository.Current.SelectAll();
                return todasFamilias.Where(f => f.EsRol).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener los roles disponibles", ex);
            }
        }

        /// <summary>
        /// Obtiene el rol (Familia) de un usuario
        /// </summary>
        public static Familia ObtenerRolDelUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }
                return usuario.ObtenerFamiliaRol();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        #region Gestión de Familias

        /// <summary>
        /// Obtiene todas las familias del sistema
        /// </summary>
        public static IEnumerable<Familia> ObtenerTodasLasFamilias()
        {
            try
            {
                return DAL.Implementations.FamiliaRepository.Current.SelectAll();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener las familias", ex);
            }
        }

        /// <summary>
        /// Obtiene las familias asignadas a un usuario
        /// </summary>
        public static IEnumerable<Familia> ObtenerFamiliasDelUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                var usuarioFamilias = DAL.Implementations.UsuarioFamiliaRepository.Current
                    .GetChildren(usuario);

                List<Familia> familias = new List<Familia>();
                foreach (var uf in usuarioFamilias)
                {
                    var familia = DAL.Implementations.FamiliaRepository.Current.SelectOne(uf.idFamilia);
                    if (familia != null)
                    {
                        familias.Add(familia);
                    }
                }

                return familias;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Asigna una familia a un usuario
        /// </summary>
        public static void AsignarFamilia(Guid idUsuario, Guid idFamilia)
        {
            try
            {
                var usuarioFamilia = new DomainModel.Security.UsuarioFamilia
                {
                    idUsuario = idUsuario,
                    idFamilia = idFamilia
                };

                DAL.Implementations.UsuarioFamiliaRepository.Current.Insert(usuarioFamilia);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al asignar familia al usuario", ex);
            }
        }

        /// <summary>
        /// Quita una familia de un usuario
        /// </summary>
        public static void QuitarFamilia(Guid idUsuario, Guid idFamilia)
        {
            try
            {
                var usuarioFamilia = new DomainModel.Security.UsuarioFamilia
                {
                    idUsuario = idUsuario,
                    idFamilia = idFamilia
                };

                DAL.Implementations.UsuarioFamiliaRepository.Current.DeleteRelacion(usuarioFamilia);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar familia del usuario", ex);
            }
        }

        #endregion

        #region Gestión de Patentes

        /// <summary>
        /// Obtiene todas las patentes del sistema
        /// </summary>
        public static IEnumerable<Patente> ObtenerTodasLasPatentes()
        {
            try
            {
                return DAL.Implementations.PatenteRepository.Current.SelectAll();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener las patentes", ex);
            }
        }

        /// <summary>
        /// Obtiene las patentes asignadas directamente a un usuario
        /// </summary>
        public static IEnumerable<Patente> ObtenerPatentesDelUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                var usuarioPatentes = DAL.Implementations.UsuarioPatenteRepository.Current
                    .GetChildren(usuario);

                List<Patente> patentes = new List<Patente>();
                foreach (var up in usuarioPatentes)
                {
                    var patente = DAL.Implementations.PatenteRepository.Current.SelectOne(up.idPatente);
                    if (patente != null)
                    {
                        patentes.Add(patente);
                    }
                }

                return patentes;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Asigna una patente directamente a un usuario
        /// </summary>
        public static void AsignarPatente(Guid idUsuario, Guid idPatente)
        {
            try
            {
                var usuarioPatente = new DomainModel.Security.UsuarioPatente
                {
                    idUsuario = idUsuario,
                    idPatente = idPatente
                };

                DAL.Implementations.UsuarioPatenteRepository.Current.Insert(usuarioPatente);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al asignar patente al usuario", ex);
            }
        }

        /// <summary>
        /// Quita una patente de un usuario
        /// </summary>
        public static void QuitarPatente(Guid idUsuario, Guid idPatente)
        {
            try
            {
                var usuarioPatente = new DomainModel.Security.UsuarioPatente
                {
                    idUsuario = idUsuario,
                    idPatente = idPatente
                };

                DAL.Implementations.UsuarioPatenteRepository.Current.DeleteRelacion(usuarioPatente);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar patente del usuario", ex);
            }
        }

        #endregion

        #region Gestión de Perfil de Usuario

        /// <summary>
        /// Actualiza el perfil del usuario (idioma preferido y opcionalmente contraseña)
        /// Este método es para que un usuario actualice su propio perfil
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="idiomaPreferido">Idioma preferido (ej: "es-AR", "en-GB")</param>
        /// <param name="nuevaPassword">Nueva contraseña (opcional, dejar null o vacío para no cambiar)</param>
        /// <param name="passwordActual">Contraseña actual (requerida si se cambia la contraseña)</param>
        public static void ActualizarPerfil(Guid idUsuario, string idiomaPreferido, string nuevaPassword = null, string passwordActual = null)
        {
            try
            {
                // Obtener usuario actual
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                // Variables para registrar los cambios en la bitácora
                List<string> cambiosRealizados = new List<string>();
                string idiomaAnterior = usuario.IdiomaPreferido;

                // Si se proporciona nueva contraseña, validar la actual
                if (!string.IsNullOrWhiteSpace(nuevaPassword))
                {
                    ValidarLongitudMinima(nuevaPassword, "Nueva contraseña", 6);

                    if (string.IsNullOrWhiteSpace(passwordActual))
                    {
                        throw new ValidacionException("Debe proporcionar la contraseña actual para cambiarla");
                    }

                    // Verificar que la contraseña actual sea correcta
                    string hashPasswordActual = CryptographyService.HashPassword(passwordActual);
                    if (usuario.Clave != hashPasswordActual)
                    {
                        // Registrar intento fallido de cambio de contraseña en bitácora de seguridad
                        DAL.Implementations.BitacoraSeguridadRepository.Current.RegistrarEventoSeguridad(
                            usuario.IdUsuario,
                            usuario.Nombre,
                            "Mi Perfil",
                            "Intento de cambio de contraseña",
                            "Intento fallido de cambio de contraseña. Contraseña actual incorrecta",
                            "Alto"
                        );
                        throw new ContraseñaInvalidaException("La contraseña actual es incorrecta");
                    }

                    // Actualizar contraseña
                    usuario.Password = nuevaPassword;
                    usuario.Clave = CryptographyService.HashPassword(nuevaPassword);
                    cambiosRealizados.Add("contraseña modificada");
                }

                // Actualizar idioma preferido
                if (!string.IsNullOrWhiteSpace(idiomaPreferido) && idiomaPreferido != idiomaAnterior)
                {
                    usuario.IdiomaPreferido = idiomaPreferido;
                    cambiosRealizados.Add($"idioma cambiado de '{idiomaAnterior ?? "no definido"}' a '{idiomaPreferido}'");
                }

                // Solo actualizar si hubo cambios
                if (cambiosRealizados.Count > 0)
                {
                    DAL.Implementations.UsuarioRepository.Current.Update(usuario);

                    // Registrar en bitácora de seguridad con detalle de todos los cambios
                    string detallesCambios = string.Join(", ", cambiosRealizados);

                    // Determinar el tipo de evento y gravedad según los cambios
                    bool incluyeCambioPassword = cambiosRealizados.Any(c => c.Contains("contraseña"));
                    string tipoEvento = incluyeCambioPassword ? "CambioCritico" : "Seguridad";
                    string gravedad = incluyeCambioPassword ? "Alto" : "Medio";

                    DAL.Implementations.BitacoraSeguridadRepository.Current.Registrar(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        tipoEvento,
                        "Mi Perfil",
                        "Actualización de perfil",
                        $"Perfil actualizado exitosamente. Cambios: {detallesCambios}",
                        gravedad
                    );
                }
            }
            catch (ContraseñaInvalidaException)
            {
                // Re-lanzar sin manejar, ya fue registrado en la bitácora
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el idioma preferido de un usuario
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Código de idioma (ej: "es-AR", "en-GB") o "es-AR" por defecto</returns>
        public static string ObtenerIdiomaPreferido(Guid idUsuario)
        {
            try
            {
                var usuario = DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                return usuario.IdiomaPreferido ?? "es-AR";
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        #endregion

        #region Métodos de Validación Privados

        private static void ValidarCampoRequerido(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidacionException($"El campo '{fieldName}' es requerido");
            }
        }

        private static void ValidarLongitudMinima(string value, string fieldName, int minLength)
        {
            if (value != null && value.Length < minLength)
            {
                throw new ValidacionException($"El campo '{fieldName}' debe tener al menos {minLength} caracteres");
            }
        }

        #endregion
    }
}
