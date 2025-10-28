using System;
using System.Collections.Generic;
using DAL.Implementations;
using Model.DomainModel.DTOs;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para reportes
    /// Responsabilidad: Obtener y validar datos de reportes
    /// Para exportar datos, utilizar Services.ExportService
    /// </summary>
    public class ReporteBLL
    {
        private readonly ReporteRepository _repository;

        // Constructor para producción
        public ReporteBLL()
        {
            _repository = new ReporteRepository();
        }

        // Constructor para testing (inyección de dependencias)
        public ReporteBLL(ReporteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene reporte de préstamos activos
        /// </summary>
        public List<ReportePrestamo> ObtenerReportePrestamosActivos(DateTime? fechaDesde, DateTime? fechaHasta, string estado = null)
        {
            return _repository.ObtenerPrestamosActivos(fechaDesde, fechaHasta, estado);
        }

        /// <summary>
        /// Obtiene reporte de materiales más prestados
        /// </summary>
        public List<ReporteEstadisticaMaterial> ObtenerReporteMaterialesMasPrestados(int top = 20)
        {
            if (top <= 0 || top > 100)
                throw new ArgumentException("El parámetro 'top' debe estar entre 1 y 100");

            return _repository.ObtenerMaterialesMasPrestados(top);
        }

        /// <summary>
        /// Obtiene reporte de inventario de ejemplares
        /// </summary>
        public List<ReporteInventarioEjemplar> ObtenerReporteInventarioEjemplares()
        {
            return _repository.ObtenerInventarioEjemplares();
        }

        /// <summary>
        /// Obtiene reporte de uso de biblioteca por grado/división
        /// </summary>
        public List<ReporteUsoPorGrado> ObtenerReporteUsoPorGrado(int? anioLectivo = null)
        {
            // Si no se especifica año lectivo, usar el actual
            if (!anioLectivo.HasValue)
            {
                anioLectivo = DateTime.Now.Year;
            }

            // Validar que el año lectivo sea razonable
            if (anioLectivo < 2000 || anioLectivo > DateTime.Now.Year + 1)
            {
                throw new ArgumentException("El año lectivo debe estar entre 2000 y el próximo año");
            }

            return _repository.ObtenerUsoPorGrado(anioLectivo);
        }
    }
}
