using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using DAL.Tools;
using DomainModel;
using Services.Services;

namespace DAL.Implementations
{
    /// <summary>
    /// Repositorio para gestionar backups (copias de seguridad) de bases de datos.
    /// Maneja tanto el catálogo de backups como la ejecución física de BACKUP/RESTORE.
    /// </summary>
    public class BackupRepository
    {
        #region Singleton
        private readonly static BackupRepository _instance = new BackupRepository();

        public static BackupRepository Current
        {
            get { return _instance; }
        }

        private BackupRepository()
        {
            // Inicialización
        }
        #endregion

        #region Catálogo de Backups

        /// <summary>
        /// Registra un backup en el catálogo
        /// </summary>
        public int RegistrarBackup(Backup backup)
        {
            try
            {
                string query = @"
                    INSERT INTO [Backup] (FechaCreacion, NombreBaseDatos, RutaArchivo, TipoBackup,
                                       TamanoMB, Estado, MensajeError, IdUsuarioCreacion, Descripcion)
                    VALUES (@FechaCreacion, @NombreBaseDatos, @RutaArchivo, @TipoBackup,
                           @TamanoMB, @Estado, @MensajeError, @IdUsuarioCreacion, @Descripcion);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FechaCreacion", backup.FechaCreacion);
                        cmd.Parameters.AddWithValue("@NombreBaseDatos", backup.NombreBaseDatos);
                        cmd.Parameters.AddWithValue("@RutaArchivo", backup.RutaArchivo);
                        cmd.Parameters.AddWithValue("@TipoBackup", backup.TipoBackup);
                        cmd.Parameters.AddWithValue("@TamanoMB", (object)backup.TamañoMB ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Estado", backup.Estado);
                        cmd.Parameters.AddWithValue("@MensajeError", (object)backup.MensajeError ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IdUsuarioCreacion", (object)backup.IdUsuarioCreacion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Descripcion", (object)backup.Descripcion ?? DBNull.Value);

                        connection.Open();
                        int idBackup = (int)cmd.ExecuteScalar();
                        backup.IdBackup = idBackup;
                        return idBackup;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar backup en el catálogo: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el estado de un backup existente
        /// </summary>
        public void ActualizarEstadoBackup(int idBackup, string estado, decimal? tamañoMB, string mensajeError = null)
        {
            try
            {
                string query = @"
                    UPDATE [Backup]
                    SET Estado = @Estado,
                        TamanoMB = @TamanoMB,
                        MensajeError = @MensajeError
                    WHERE IdBackup = @IdBackup";

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdBackup", idBackup);
                        cmd.Parameters.AddWithValue("@Estado", estado);
                        cmd.Parameters.AddWithValue("@TamanoMB", (object)tamañoMB ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MensajeError", (object)mensajeError ?? DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estado del backup: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene todos los backups registrados
        /// </summary>
        public List<Backup> ObtenerTodos()
        {
            try
            {
                string query = @"
                    SELECT b.*, u.Nombre as NombreUsuario
                    FROM [Backup] b
                    LEFT JOIN Usuario u ON b.IdUsuarioCreacion = u.IdUsuario
                    ORDER BY b.FechaCreacion DESC";

                List<Backup> backups = new List<Backup>();

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                backups.Add(MapearBackup(reader));
                            }
                        }
                    }
                }

                return backups;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener lista de backups: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene backups por nombre de base de datos
        /// </summary>
        public List<Backup> ObtenerPorBaseDatos(string nombreBaseDatos)
        {
            try
            {
                string query = @"
                    SELECT b.*, u.Nombre as NombreUsuario
                    FROM [Backup] b
                    LEFT JOIN Usuario u ON b.IdUsuarioCreacion = u.IdUsuario
                    WHERE b.NombreBaseDatos = @NombreBaseDatos
                    ORDER BY b.FechaCreacion DESC";

                List<Backup> backups = new List<Backup>();

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@NombreBaseDatos", nombreBaseDatos);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                backups.Add(MapearBackup(reader));
                            }
                        }
                    }
                }

                return backups;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener backups por base de datos: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene un backup por ID
        /// </summary>
        public Backup ObtenerPorId(int idBackup)
        {
            try
            {
                string query = @"
                    SELECT b.*, u.Nombre as NombreUsuario
                    FROM [Backup] b
                    LEFT JOIN Usuario u ON b.IdUsuarioCreacion = u.IdUsuario
                    WHERE b.IdBackup = @IdBackup";

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdBackup", idBackup);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapearBackup(reader);
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener backup por ID: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de backup del catálogo (no elimina el archivo físico)
        /// </summary>
        public void EliminarRegistro(int idBackup)
        {
            try
            {
                string query = "DELETE FROM [Backup] WHERE IdBackup = @IdBackup";

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdBackup", idBackup);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar registro de backup: " + ex.Message, ex);
            }
        }

        #endregion

        #region Operaciones Físicas de Backup/Restore

        /// <summary>
        /// Ejecuta un backup físico de la base de datos
        /// </summary>
        public void EjecutarBackup(string nombreBaseDatos, string rutaArchivo, string tipoBackup = "Full")
        {
            try
            {
                // Asegurar que el directorio existe
                string directorio = Path.GetDirectoryName(rutaArchivo);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                string query = "";

                if (tipoBackup == "Full")
                {
                    query = $"BACKUP DATABASE [{nombreBaseDatos}] TO DISK = @RutaArchivo WITH INIT, FORMAT, NAME = @NombreBackup";
                }
                else if (tipoBackup == "Differential")
                {
                    query = $"BACKUP DATABASE [{nombreBaseDatos}] TO DISK = @RutaArchivo WITH DIFFERENTIAL, INIT, FORMAT, NAME = @NombreBackup";
                }

                using (SqlConnection connection = SqlHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandTimeout = 300; // 5 minutos timeout
                        cmd.Parameters.AddWithValue("@RutaArchivo", rutaArchivo);
                        cmd.Parameters.AddWithValue("@NombreBackup", $"{nombreBaseDatos}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}");

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar backup físico: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Restaura una base de datos desde un archivo de backup
        /// ADVERTENCIA: Esta operación sobrescribirá la base de datos actual
        /// </summary>
        public void EjecutarRestore(string nombreBaseDatos, string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    throw new FileNotFoundException("El archivo de backup no existe: " + rutaArchivo);
                }

                // Usar conexión a master para poder poner la BD en modo single user
                string connectionString = SqlHelper.GetConnection().ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.InitialCatalog = "master";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    // 1. Cerrar todas las conexiones activas a la BD
                    string killConnections = $@"
                        ALTER DATABASE [{nombreBaseDatos}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

                    using (SqlCommand cmd = new SqlCommand(killConnections, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Ejecutar RESTORE
                    string restoreQuery = $@"
                        RESTORE DATABASE [{nombreBaseDatos}]
                        FROM DISK = @RutaArchivo
                        WITH REPLACE;";

                    using (SqlCommand cmd = new SqlCommand(restoreQuery, connection))
                    {
                        cmd.CommandTimeout = 600; // 10 minutos timeout
                        cmd.Parameters.AddWithValue("@RutaArchivo", rutaArchivo);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Volver a modo multi-user
                    string setMultiUser = $@"
                        ALTER DATABASE [{nombreBaseDatos}] SET MULTI_USER;";

                    using (SqlCommand cmd = new SqlCommand(setMultiUser, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Intentar restaurar el modo multi-user si falla
                try
                {
                    string connectionString = SqlHelper.GetConnection().ConnectionString;
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                    builder.InitialCatalog = "master";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        connection.Open();
                        string setMultiUser = $"ALTER DATABASE [{nombreBaseDatos}] SET MULTI_USER;";
                        using (SqlCommand cmd = new SqlCommand(setMultiUser, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch { /* Ignorar si no se puede restaurar */ }

                throw new Exception("Error al ejecutar restore: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el tamaño de un archivo de backup en MB
        /// </summary>
        public decimal ObtenerTamañoArchivo(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                    return 0;

                FileInfo fileInfo = new FileInfo(rutaArchivo);
                return (decimal)(fileInfo.Length / (1024.0 * 1024.0)); // Convertir a MB
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Mapea un SqlDataReader a un objeto Backup
        /// </summary>
        private Backup MapearBackup(SqlDataReader reader)
        {
            return new Backup
            {
                IdBackup = reader.GetInt32(reader.GetOrdinal("IdBackup")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                NombreBaseDatos = reader.GetString(reader.GetOrdinal("NombreBaseDatos")),
                RutaArchivo = reader.GetString(reader.GetOrdinal("RutaArchivo")),
                TipoBackup = reader.GetString(reader.GetOrdinal("TipoBackup")),
                TamañoMB = reader.IsDBNull(reader.GetOrdinal("TamanoMB")) ?
                          (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TamanoMB")),
                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                MensajeError = reader.IsDBNull(reader.GetOrdinal("MensajeError")) ?
                              null : reader.GetString(reader.GetOrdinal("MensajeError")),
                IdUsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("IdUsuarioCreacion")) ?
                                   (Guid?)null : reader.GetGuid(reader.GetOrdinal("IdUsuarioCreacion")),
                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ?
                               null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ?
                             null : reader.GetString(reader.GetOrdinal("Descripcion"))
            };
        }

        #endregion
    }
}
