using System;

namespace DomainModel
{
    /// <summary>
    /// Entidad que representa un registro de bitácora de seguridad.
    /// Almacena eventos críticos: errores del sistema, acciones de seguridad y cambios importantes.
    /// </summary>
    public class BitacoraSeguridad
    {
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public Guid? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoEvento { get; set; } // 'Error', 'Seguridad', 'CambioCritico'
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public string Gravedad { get; set; } // 'Bajo', 'Medio', 'Alto'
        public string DireccionIP { get; set; }

        public BitacoraSeguridad()
        {
            Fecha = DateTime.Now;
            Gravedad = "Medio";
        }
    }

    /// <summary>
    /// Enumeración para tipos de eventos de seguridad
    /// </summary>
    public enum TipoEventoSeguridad
    {
        Error,
        Seguridad,
        CambioCritico
    }

    /// <summary>
    /// Enumeración para niveles de gravedad
    /// </summary>
    public enum NivelGravedad
    {
        Bajo,
        Medio,
        Alto
    }
}
