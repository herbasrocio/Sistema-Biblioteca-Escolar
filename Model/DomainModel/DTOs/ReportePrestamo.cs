using System;

namespace Model.DomainModel.DTOs
{
    /// <summary>
    /// DTO para reporte de préstamos activos
    /// </summary>
    public class ReportePrestamo
    {
        public Guid IdPrestamo { get; set; }
        public string Alumno { get; set; }
        public string DNI { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string CodigoEjemplar { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public int DiasRestantes { get; set; }
        public string EstadoPrestamo { get; set; }
        public string Grado { get; set; }
        public string Division { get; set; }
    }
}
