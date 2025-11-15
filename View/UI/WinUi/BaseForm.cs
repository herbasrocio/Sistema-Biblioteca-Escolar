using System;
using System.Windows.Forms;
using ServicesSecurity.Services;

namespace UI.WinUi
{
    /// <summary>
    /// Clase base para todos los formularios de la aplicación
    /// Implementa el patrón Observer para cambios de idioma automáticos
    /// </summary>
    public class BaseForm : Form
    {
        private bool _isSubscribed = false;

        public BaseForm()
        {
            // Suscribirse al evento de cambio de idioma
            SubscribeToLanguageChanges();

            // Al cargar el formulario, aplicar traducciones
            this.Load += BaseForm_Load;
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            // Aplicar traducciones iniciales
            AplicarTraducciones();
        }

        /// <summary>
        /// Suscribe el formulario al evento de cambio de idioma
        /// </summary>
        private void SubscribeToLanguageChanges()
        {
            if (!_isSubscribed)
            {
                LanguageManager.LanguageChanged += OnLanguageChanged;
                _isSubscribed = true;
            }
        }

        /// <summary>
        /// Desuscribe el formulario del evento de cambio de idioma
        /// </summary>
        private void UnsubscribeFromLanguageChanges()
        {
            if (_isSubscribed)
            {
                LanguageManager.LanguageChanged -= OnLanguageChanged;
                _isSubscribed = false;
            }
        }

        /// <summary>
        /// Manejador del evento de cambio de idioma
        /// Llama a AplicarTraducciones() automáticamente
        /// </summary>
        private void OnLanguageChanged(object sender, LanguageChangedEventArgs e)
        {
            // Verificar que el formulario no esté cerrado o en proceso de cierre
            if (!this.IsDisposed && !this.Disposing)
            {
                // Usar Invoke para asegurar que se ejecuta en el thread de UI
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => AplicarTraducciones()));
                }
                else
                {
                    AplicarTraducciones();
                }
            }
        }

        /// <summary>
        /// Método virtual que debe ser sobrescrito por las clases derivadas
        /// para aplicar las traducciones a sus controles
        /// </summary>
        protected virtual void AplicarTraducciones()
        {
            // Las clases derivadas deben sobrescribir este método
            // para aplicar las traducciones específicas de cada formulario
        }

        /// <summary>
        /// Sobrescribir Dispose para limpiar la suscripción al evento
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnsubscribeFromLanguageChanges();
            }
            base.Dispose(disposing);
        }
    }
}
