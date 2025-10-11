using System;

namespace DomainModel
{
    /// <summary>
    /// Representa un año lectivo con sus fechas y estado
    /// </summary>
    public class AnioLectivo
    {
        public int Anio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } // Activo, Cerrado, Planificado

        public AnioLectivo()
        {
            Anio = DateTime.Now.Year;
            Estado = "Planificado";
        }

        /// <summary>
        /// Determina si este año lectivo está actualmente en curso
        /// </summary>
        public bool EsActivo => Estado == "Activo" &&
                                DateTime.Now >= FechaInicio &&
                                DateTime.Now <= FechaFin;

        /// <summary>
        /// Calcula la duración del año lectivo en días
        /// </summary>
        public int DuracionEnDias => (FechaFin - FechaInicio).Days;

        public override string ToString()
        {
            return $"Año Lectivo {Anio} ({Estado})";
        }
    }
}
