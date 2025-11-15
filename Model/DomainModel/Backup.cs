using System;

namespace DomainModel
{
    /// <summary>
    /// Entidad que representa un backup (copia de seguridad) de una base de datos.
    /// Almacena información sobre backups realizados y permite gestionar el catálogo de copias de seguridad.
    /// </summary>
    public class Backup
    {
        public int IdBackup { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NombreBaseDatos { get; set; }
        public string RutaArchivo { get; set; }
        public string TipoBackup { get; set; } // 'Full', 'Differential', 'Transaction Log'
        public decimal? TamañoMB { get; set; }
        public string Estado { get; set; } // 'Exitoso', 'Fallido', 'En Proceso'
        public string MensajeError { get; set; }
        public Guid? IdUsuarioCreacion { get; set; }
        public string NombreUsuario { get; set; } // Para mostrar en grillas
        public string Descripcion { get; set; }

        public Backup()
        {
            FechaCreacion = DateTime.Now;
            TipoBackup = "Full";
            Estado = "En Proceso";
        }

        /// <summary>
        /// Obtiene el nombre del archivo sin la ruta completa
        /// </summary>
        public string NombreArchivo
        {
            get
            {
                if (string.IsNullOrEmpty(RutaArchivo))
                    return string.Empty;

                return System.IO.Path.GetFileName(RutaArchivo);
            }
        }

        /// <summary>
        /// Verifica si el backup fue exitoso
        /// </summary>
        public bool EsExitoso
        {
            get { return Estado == "Exitoso"; }
        }

        /// <summary>
        /// Obtiene el tamaño formateado en MB
        /// </summary>
        public string TamañoFormateado
        {
            get
            {
                if (!TamañoMB.HasValue)
                    return "N/A";

                if (TamañoMB.Value < 1)
                    return $"{(TamañoMB.Value * 1024):N2} KB";
                else if (TamañoMB.Value > 1024)
                    return $"{(TamañoMB.Value / 1024):N2} GB";
                else
                    return $"{TamañoMB.Value:N2} MB";
            }
        }
    }

    /// <summary>
    /// Enumeración para tipos de backup
    /// </summary>
    public enum TipoBackup
    {
        Full,
        Differential,
        TransactionLog
    }

    /// <summary>
    /// Enumeración para estados de backup
    /// </summary>
    public enum EstadoBackup
    {
        EnProceso,
        Exitoso,
        Fallido
    }
}
