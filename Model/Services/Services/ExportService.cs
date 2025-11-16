using System;
using DomainModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Model.DomainModel.DTOs;

namespace Services.Services
{
    /// <summary>
    /// Servicio de utilidad para exportar datos a diferentes formatos
    /// </summary>
    public class ExportService
    {
        /// <summary>
        /// Exporta préstamos a CSV
        /// </summary>
        public void ExportarPrestamosCsv(List<ReportePrestamo> prestamos, string rutaArchivo)
        {
            if (prestamos == null || prestamos.Count == 0)
                throw new ArgumentException("No hay datos para exportar");

            if (string.IsNullOrEmpty(rutaArchivo))
                throw new ArgumentException("Ruta de archivo inválida");

            using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
            {
                // Header
                writer.WriteLine("Alumno,DNI,Título,Autor,Código Ejemplar,Fecha Préstamo,Fecha Devolución,Días Restantes,Estado,Grado,División");

                // Data
                foreach (var p in prestamos)
                {
                    writer.WriteLine($"\"{p.Alumno}\",\"{p.DNI}\"," +
                                   $"\"{p.Titulo}\",\"{p.Autor}\",\"{p.CodigoEjemplar}\"," +
                                   $"{p.FechaPrestamo:yyyy-MM-dd},{p.FechaDevolucionEsperada:yyyy-MM-dd}," +
                                   $"{p.DiasRestantes},\"{p.EstadoPrestamo}\",\"{p.Grado}\",\"{p.Division}\"");
                }
            }
        }

        /// <summary>
        /// Exporta estadísticas de materiales a CSV
        /// </summary>
        public void ExportarEstadisticasMaterialesCsv(List<ReporteEstadisticaMaterial> materiales, string rutaArchivo)
        {
            if (materiales == null || materiales.Count == 0)
                throw new ArgumentException("No hay datos para exportar");

            if (string.IsNullOrEmpty(rutaArchivo))
                throw new ArgumentException("Ruta de archivo inválida");

            using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
            {
                // Header
                writer.WriteLine("Título,Autor,Género,Nivel,Total Ejemplares,Préstamos Último Mes,Préstamos Último Año,Total Préstamos");

                // Data
                foreach (var m in materiales)
                {
                    writer.WriteLine($"\"{m.Titulo}\",\"{m.Autor}\",\"{m.Genero}\",\"{m.Nivel}\"," +
                                   $"{m.TotalEjemplares},{m.PrestamosUltimoMes},{m.PrestamosUltimoAnio},{m.TotalPrestamos}");
                }
            }
        }

        /// <summary>
        /// Exporta inventario de ejemplares a CSV
        /// </summary>
        public void ExportarInventarioEjemplaresCsv(List<ReporteInventarioEjemplar> inventario, string rutaArchivo)
        {
            if (inventario == null || inventario.Count == 0)
                throw new ArgumentException("No hay datos para exportar");

            if (string.IsNullOrEmpty(rutaArchivo))
                throw new ArgumentException("Ruta de archivo inválida");

            using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
            {
                // Header
                writer.WriteLine("Título,Autor,Género,Total Ejemplares,Disponibles,Prestados,Mantenimiento,Perdidos,% Disponibilidad");

                // Data
                foreach (var i in inventario)
                {
                    writer.WriteLine($"\"{i.Titulo}\",\"{i.Autor}\",\"{i.Genero}\"," +
                                   $"{i.TotalEjemplares},{i.Disponibles},{i.Prestados}," +
                                   $"{i.Mantenimiento},{i.Perdidos},{i.PorcentajeDisponibilidad:F2}");
                }
            }
        }

        /// <summary>
        /// Exporta reporte de uso por grado a CSV
        /// </summary>
        public void ExportarUsoPorGradoCsv(List<ReporteUsoPorGrado> reportes, string rutaArchivo)
        {
            if (reportes == null || reportes.Count == 0)
                throw new ArgumentException("No hay datos para exportar");

            if (string.IsNullOrEmpty(rutaArchivo))
                throw new ArgumentException("Ruta de archivo inválida");

            using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
            {
                // Header
                writer.WriteLine("Grado,División,Cantidad Alumnos,Total Préstamos,Préstamos Activos,Préstamos Devueltos,Préstamos Vencidos,Género Más Prestado,Material Más Prestado");

                // Data
                foreach (var r in reportes)
                {
                    writer.WriteLine($"\"{r.Grado}\",\"{r.Division}\"," +
                                   $"{r.CantidadAlumnos},{r.TotalPrestamos},{r.PrestamosActivos}," +
                                   $"{r.PrestamosDevueltos},{r.PrestamosVencidos}," +
                                   $"\"{r.GeneroMasPrestado}\",\"{r.MaterialMasPrestado}\"");
                }
            }
        }

        /// <summary>
        /// Exporta cualquier lista genérica a CSV (método genérico para futuros usos)
        /// </summary>
        public void ExportarACsv<T>(List<T> datos, string rutaArchivo, Func<T, string> formatearLinea, string header)
        {
            if (datos == null || datos.Count == 0)
                throw new ArgumentException("No hay datos para exportar");

            if (string.IsNullOrEmpty(rutaArchivo))
                throw new ArgumentException("Ruta de archivo inválida");

            using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
            {
                // Header
                if (!string.IsNullOrEmpty(header))
                    writer.WriteLine(header);

                // Data
                foreach (var item in datos)
                {
                    writer.WriteLine(formatearLinea(item));
                }
            }
        }
    }
}