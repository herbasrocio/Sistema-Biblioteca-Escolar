using System;

namespace DomainModel
{
    /// <summary>
    /// Entidad que representa un registro de bitácora de administrador.
    /// Almacena eventos críticos: errores del sistema, acciones de seguridad y cambios importantes.
    /// </summary>
    public class BitacoraAdmin
    {
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public Guid? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoEvento { get; set; } // 'Error', 'Seguridad', 'CambioCritico'
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public string Criticidad { get; set; } // 'Baja', 'Media', 'Alta', 'Critica'
        public string DireccionIP { get; set; }

        public BitacoraAdmin()
        {
            Fecha = DateTime.Now;
            Criticidad = "Media";
        }
    }

    /// <summary>
    /// Enumeración para tipos de eventos de administrador
    /// </summary>
    public enum TipoEventoAdmin
    {
        Error,
        Seguridad,
        CambioCritico
    }

    /// <summary>
    /// Enumeración para niveles de criticidad
    /// </summary>
    public enum NivelCriticidad
    {
        Baja,
        Media,
        Alta,
        Critica
    }
}
