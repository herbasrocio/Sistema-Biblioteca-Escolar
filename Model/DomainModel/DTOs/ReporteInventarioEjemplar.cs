using System;

namespace Model.DomainModel.DTOs
{
    /// <summary>
    /// DTO para reporte de inventario de ejemplares por estado
    /// </summary>
    public class ReporteInventarioEjemplar
    {
        public Guid IdMaterial { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public int TotalEjemplares { get; set; }
        public int Disponibles { get; set; }
        public int Prestados { get; set; }
        public int Mantenimiento { get; set; }
        public int Perdidos { get; set; }
        public decimal PorcentajeDisponibilidad { get; set; }
    }
}
