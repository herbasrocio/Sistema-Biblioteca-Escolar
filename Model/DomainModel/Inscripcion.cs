using System;

namespace DomainModel
{
    /// <summary>
    /// Representa la inscripción de un alumno en un año lectivo específico
    /// </summary>
    public class Inscripcion
    {
        public Guid IdInscripcion { get; set; }
        public Guid IdAlumno { get; set; }
        public int AnioLectivo { get; set; }
        public string Grado { get; set; }
        public string Division { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; } // Activo, Finalizado, Abandonado

        // Propiedades de navegación
        public Alumno Alumno { get; set; }

        public Inscripcion()
        {
            IdInscripcion = Guid.NewGuid();
            FechaInscripcion = DateTime.Now;
            Estado = "Activo";
            AnioLectivo = DateTime.Now.Year;
        }

        /// <summary>
        /// Determina si esta inscripción es la activa para el año actual
        /// </summary>
        public bool EsActiva => Estado == "Activo" && AnioLectivo == DateTime.Now.Year;

        public override string ToString()
        {
            return $"{Grado}° {Division} - Año {AnioLectivo} ({Estado})";
        }
    }
}
