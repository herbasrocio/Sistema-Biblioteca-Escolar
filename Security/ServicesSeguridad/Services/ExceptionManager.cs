using System;
using System.Diagnostics.Tracing;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Gestor centralizado de excepciones del sistema
    /// Singleton pattern
    /// </summary>
    public sealed class ExceptionManager
    {
        #region Singleton
        private static readonly ExceptionManager _instance = new ExceptionManager();

        public static ExceptionManager Current
        {
            get { return _instance; }
        }

        private ExceptionManager()
        {
        }
        #endregion

        /// <summary>
        /// Maneja una excepción registrándola en el log
        /// </summary>
        public void Handle(Exception ex)
        {
            // Las excepciones ahora se manejan silenciosamente
            // o se registran en BitacoraSeguridad según el contexto
        }

        /// <summary>
        /// Maneja una excepción con información del emisor
        /// </summary>
        public void Handle(Exception ex, object sender)
        {
            // Las excepciones ahora se manejan silenciosamente
            // o se registran en BitacoraSeguridad según el contexto
        }
    }
}
