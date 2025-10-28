using System;

namespace Model.DomainModel.DTOs
{
    /// <summary>
    /// DTO para reporte de uso de biblioteca por grado/división
    /// </summary>
    public class ReporteUsoPorGrado
    {
        public string Grado { get; set; }
        public string Division { get; set; }
        public int CantidadAlumnos { get; set; }
        public int TotalPrestamos { get; set; }
        public int PrestamosActivos { get; set; }
        public int PrestamosDevueltos { get; set; }
        public int PrestamosVencidos { get; set; }
        public string GeneroMasPrestado { get; set; }
        public string MaterialMasPrestado { get; set; }
    }
}
