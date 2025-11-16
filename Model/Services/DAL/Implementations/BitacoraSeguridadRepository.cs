using DAL.Tools;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementations
{
    /// <summary>
    /// Repositorio para registrar eventos en la bitácora de seguridad
    /// Escribe directamente en la base de datos SeguridadBiblioteca
    /// </summary>
    internal sealed class BitacoraSeguridadRepository
    {
        #region Singleton
        private readonly static BitacoraSeguridadRepository _instance = new BitacoraSeguridadRepository();

        public static BitacoraSeguridadRepository Current
        {
            get { return _instance; }
        }

        private BitacoraSeguridadRepository() { }
        #endregion

        /// <summary>
        /// Registra un evento en la bitácora de seguridad
        /// </summary>
        /// <param name="idUsuario">ID del usuario (opcional)</param>
        /// <param name="nombreUsuario">Nombre del usuario</param>
        /// <param name="tipoEvento">Tipo de evento: Error, Seguridad, CambioCritico</param>
        /// <param name="modulo">Módulo donde ocurrió el evento</param>
        /// <param name="accion">Acción realizada</param>
        /// <param name="detalle">Detalle del evento</param>
        /// <param name="gravedad">Gravedad: Bajo, Medio, Alto</param>
        public void Registrar(Guid? idUsuario, string nombreUsuario, string tipoEvento,
            string modulo, string accion, string detalle, string gravedad = "Medio")
        {
            try
            {
                string query = @"INSERT INTO BitacoraSeguridad
                    (Fecha, IdUsuario, NombreUsuario, TipoEvento, Modulo, Accion, Detalle, Gravedad)
                    VALUES
                    (@Fecha, @IdUsuario, @NombreUsuario, @TipoEvento, @Modulo, @Accion, @Detalle, @Gravedad)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Fecha", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@IdUsuario", SqlDbType.UniqueIdentifier) { Value = (object)idUsuario ?? DBNull.Value },
                    new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 100) { Value = (object)nombreUsuario ?? DBNull.Value },
                    new SqlParameter("@TipoEvento", SqlDbType.NVarChar, 50) { Value = tipoEvento },
                    new SqlParameter("@Modulo", SqlDbType.NVarChar, 100) { Value = modulo },
                    new SqlParameter("@Accion", SqlDbType.NVarChar, 200) { Value = accion },
                    new SqlParameter("@Detalle", SqlDbType.NVarChar) { Value = (object)detalle ?? DBNull.Value },
                    new SqlParameter("@Gravedad", SqlDbType.NVarChar, 20) { Value = gravedad }
                };

                SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
            }
            catch
            {
                // Silenciar errores para no afectar el flujo principal
                // En un sistema real, esto debería registrarse en un log alternativo
            }
        }

        /// <summary>
        /// Registra un evento de seguridad (autenticación, permisos, etc.)
        /// </summary>
        public void RegistrarEventoSeguridad(Guid? idUsuario, string nombreUsuario, string modulo, string accion, string detalle, string gravedad = "Alto")
        {
            Registrar(idUsuario, nombreUsuario, "Seguridad", modulo, accion, detalle, gravedad);
        }

        /// <summary>
        /// Registra un cambio crítico (modificación de datos importantes)
        /// </summary>
        public void RegistrarCambioCritico(Guid? idUsuario, string nombreUsuario, string modulo, string accion, string detalle, string gravedad = "Alto")
        {
            Registrar(idUsuario, nombreUsuario, "CambioCritico", modulo, accion, detalle, gravedad);
        }

        /// <summary>
        /// Registra un error del sistema
        /// </summary>
        public void RegistrarError(Guid? idUsuario, string nombreUsuario, string modulo, string accion, string detalle, string gravedad = "Alto")
        {
            Registrar(idUsuario, nombreUsuario, "Error", modulo, accion, detalle, gravedad);
        }
    }
}
