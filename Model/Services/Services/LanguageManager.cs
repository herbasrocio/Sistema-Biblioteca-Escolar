using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    /// <summary>
    /// EventArgs para el evento de cambio de idioma
    /// </summary>
    public class LanguageChangedEventArgs : EventArgs
    {
        public string NewCulture { get; private set; }
        public string PreviousCulture { get; private set; }

        public LanguageChangedEventArgs(string newCulture, string previousCulture)
        {
            NewCulture = newCulture;
            PreviousCulture = previousCulture;
        }
    }

    /// <summary>
    /// Gestor de traducciones para internacionalización (i18n)
    /// Permite traducir palabras según el idioma/cultura actual del thread
    /// Implementa el patrón Observer para notificar cambios de idioma
    /// </summary>
    public static class LanguageManager
    {
        /// <summary>
        /// Evento que se dispara cuando cambia el idioma
        /// Los formularios pueden suscribirse para actualizar sus traducciones automáticamente
        /// </summary>
        public static event EventHandler<LanguageChangedEventArgs> LanguageChanged;

        /// <summary>
        /// Traduce una palabra según el idioma actual (Thread.CurrentThread.CurrentCulture)
        /// Si la palabra no existe, la agrega al archivo y retorna la palabra original
        /// </summary>
        /// <param name="word">Palabra clave a traducir</param>
        /// <returns>Traducción de la palabra o la palabra original si no existe</returns>
        public static string Translate(string word)
        {
            return BLL.LanguageBLL.Translate(word);
        }

        /// <summary>
        /// Cambia el idioma de la aplicación y notifica a todos los observers
        /// </summary>
        /// <param name="newCulture">Código de cultura del nuevo idioma (ej: "es-AR", "en-GB")</param>
        public static void ChangeLanguage(string newCulture)
        {
            string previousCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            // Cambiar la cultura del thread actual
            var cultureInfo = new System.Globalization.CultureInfo(newCulture);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            // Notificar a todos los observers (formularios abiertos)
            OnLanguageChanged(new LanguageChangedEventArgs(newCulture, previousCulture));
        }

        /// <summary>
        /// Método interno para disparar el evento LanguageChanged
        /// </summary>
        private static void OnLanguageChanged(LanguageChangedEventArgs e)
        {
            LanguageChanged?.Invoke(null, e);
        }

        /// <summary>
        /// Obtiene todas las traducciones del archivo de idioma actual
        /// </summary>
        /// <returns>Diccionario con todas las palabras y sus traducciones</returns>
        public static Dictionary<string, string> GetAllTranslations()
        {
            return DAL.Factory.ServiceFactory.LanguageRepository.FindAll();
        }

        /// <summary>
        /// Obtiene la lista de culturas/idiomas disponibles
        /// </summary>
        /// <returns>Lista de códigos de cultura (ej: "es-AR", "en-GB")</returns>
        public static List<string> GetAvailableLanguages()
        {
            return DAL.Factory.ServiceFactory.LanguageRepository.GetCurrentCultures();
        }

        /// <summary>
        /// Obtiene el idioma/cultura actual
        /// </summary>
        /// <returns>Código de cultura actual (ej: "es-AR")</returns>
        public static string GetCurrentLanguage()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        }
    }
}
