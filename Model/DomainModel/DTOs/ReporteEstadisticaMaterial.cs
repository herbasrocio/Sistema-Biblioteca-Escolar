using System;

namespace Model.DomainModel.DTOs
{
    /// <summary>
    /// DTO para reporte de materiales más prestados
    /// </summary>
    public class ReporteEstadisticaMaterial
    {
        public Guid IdMaterial { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public string Nivel { get; set; }
        public int TotalEjemplares { get; set; }
        public int TotalPrestamos { get; set; }
        public int PrestamosUltimoMes { get; set; }
        public int PrestamosUltimoAnio { get; set; }
    }
}
