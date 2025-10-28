using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Model.DomainModel.DTOs;

namespace DAL.Implementations
{
    /// <summary>
    /// Repositorio para consultas específicas de reportes
    /// </summary>
    public class ReporteRepository
    {
        private readonly string _connectionString;

        public ReporteRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        /// <summary>
        /// Obtiene reporte de préstamos activos con filtros opcionales
        /// </summary>
        public List<ReportePrestamo> ObtenerPrestamosActivos(DateTime? fechaDesde = null, DateTime? fechaHasta = null, string estado = null)
        {
            var reportes = new List<ReportePrestamo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        p.IdPrestamo,
                        a.Nombre + ' ' + a.Apellido AS Alumno,
                        a.DNI,
                        m.Titulo,
                        m.Autor,
                        e.CodigoEjemplar,
                        p.FechaPrestamo,
                        p.FechaDevolucionPrevista AS FechaDevolucionEsperada,
                        DATEDIFF(day, GETDATE(), p.FechaDevolucionPrevista) AS DiasRestantes,
                        CASE
                            WHEN GETDATE() > p.FechaDevolucionPrevista THEN 'VENCIDO'
                            WHEN DATEDIFF(day, GETDATE(), p.FechaDevolucionPrevista) <= 3 THEN 'POR VENCER'
                            ELSE 'VIGENTE'
                        END AS EstadoPrestamo,
                        ISNULL(i.Grado, 'SIN DATOS') AS Grado,
                        ISNULL(i.Division, 'SIN DATOS') AS Division
                    FROM Prestamo p
                    INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
                    INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
                    INNER JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
                    LEFT JOIN (
                        SELECT IdAlumno, Grado, Division, AnioLectivo
                        FROM Inscripcion
                        WHERE Estado = 'Activo'
                    ) i ON a.IdAlumno = i.IdAlumno
                    WHERE p.Estado = 'Activo'";

                if (fechaDesde.HasValue)
                    query += " AND p.FechaPrestamo >= @FechaDesde";
                if (fechaHasta.HasValue)
                    query += " AND p.FechaPrestamo <= @FechaHasta";
                if (!string.IsNullOrEmpty(estado))
                {
                    if (estado == "VENCIDO")
                        query += " AND GETDATE() > p.FechaDevolucionPrevista";
                    else if (estado == "POR VENCER")
                        query += " AND DATEDIFF(day, GETDATE(), p.FechaDevolucionPrevista) <= 3 AND GETDATE() <= p.FechaDevolucionPrevista";
                    else if (estado == "VIGENTE")
                        query += " AND DATEDIFF(day, GETDATE(), p.FechaDevolucionPrevista) > 3 AND GETDATE() <= p.FechaDevolucionPrevista";
                }

                query += " ORDER BY p.FechaDevolucionPrevista ASC";

                var command = new SqlCommand(query, connection);

                if (fechaDesde.HasValue)
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.Value);
                if (fechaHasta.HasValue)
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.Value);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reportes.Add(new ReportePrestamo
                        {
                            IdPrestamo = (Guid)reader["IdPrestamo"],
                            Alumno = reader["Alumno"].ToString(),
                            DNI = reader["DNI"].ToString(),
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"]?.ToString() ?? "",
                            CodigoEjemplar = reader["CodigoEjemplar"].ToString(),
                            FechaPrestamo = (DateTime)reader["FechaPrestamo"],
                            FechaDevolucionEsperada = (DateTime)reader["FechaDevolucionEsperada"],
                            DiasRestantes = Convert.ToInt32(reader["DiasRestantes"]),
                            EstadoPrestamo = reader["EstadoPrestamo"].ToString(),
                            Grado = reader["Grado"].ToString(),
                            Division = reader["Division"].ToString()
                        });
                    }
                }
            }

            return reportes;
        }

        /// <summary>
        /// Obtiene estadísticas de materiales más prestados
        /// </summary>
        public List<ReporteEstadisticaMaterial> ObtenerMaterialesMasPrestados(int top = 20)
        {
            var reportes = new List<ReporteEstadisticaMaterial>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = $@"
                    SELECT TOP {top}
                        m.IdMaterial,
                        m.Titulo,
                        m.Autor,
                        m.Genero,
                        m.Nivel,
                        COUNT(DISTINCT e.IdEjemplar) AS TotalEjemplares,
                        COUNT(DISTINCT p.IdPrestamo) AS TotalPrestamos,
                        COUNT(DISTINCT CASE WHEN p.FechaPrestamo >= DATEADD(month, -1, GETDATE()) THEN p.IdPrestamo END) AS PrestamosUltimoMes,
                        COUNT(DISTINCT CASE WHEN p.FechaPrestamo >= DATEADD(year, -1, GETDATE()) THEN p.IdPrestamo END) AS PrestamosUltimoAnio
                    FROM Material m
                    LEFT JOIN Ejemplar e ON m.IdMaterial = e.IdMaterial AND e.Activo = 1
                    LEFT JOIN Prestamo p ON m.IdMaterial = p.IdMaterial
                    WHERE m.Activo = 1
                    GROUP BY m.IdMaterial, m.Titulo, m.Autor, m.Genero, m.Nivel
                    ORDER BY TotalPrestamos DESC";

                var command = new SqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reportes.Add(new ReporteEstadisticaMaterial
                        {
                            IdMaterial = (Guid)reader["IdMaterial"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"]?.ToString() ?? "",
                            Genero = reader["Genero"]?.ToString() ?? "",
                            Nivel = reader["Nivel"]?.ToString() ?? "",
                            TotalEjemplares = Convert.ToInt32(reader["TotalEjemplares"]),
                            TotalPrestamos = Convert.ToInt32(reader["TotalPrestamos"]),
                            PrestamosUltimoMes = Convert.ToInt32(reader["PrestamosUltimoMes"]),
                            PrestamosUltimoAnio = Convert.ToInt32(reader["PrestamosUltimoAnio"])
                        });
                    }
                }
            }

            return reportes;
        }

        /// <summary>
        /// Obtiene reporte de inventario de ejemplares por estado
        /// </summary>
        public List<ReporteInventarioEjemplar> ObtenerInventarioEjemplares()
        {
            var reportes = new List<ReporteInventarioEjemplar>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        m.IdMaterial,
                        m.Titulo,
                        m.Autor,
                        m.Genero,
                        COUNT(e.IdEjemplar) AS TotalEjemplares,
                        SUM(CASE WHEN e.Estado = 0 THEN 1 ELSE 0 END) AS Disponibles,
                        SUM(CASE WHEN e.Estado = 1 THEN 1 ELSE 0 END) AS Prestados,
                        SUM(CASE WHEN e.Estado = 2 THEN 1 ELSE 0 END) AS Mantenimiento,
                        SUM(CASE WHEN e.Estado = 3 THEN 1 ELSE 0 END) AS Perdidos,
                        CASE
                            WHEN COUNT(e.IdEjemplar) > 0 THEN
                                CAST(SUM(CASE WHEN e.Estado = 0 THEN 1 ELSE 0 END) AS DECIMAL(5,2)) / COUNT(e.IdEjemplar) * 100
                            ELSE 0
                        END AS PorcentajeDisponibilidad
                    FROM Material m
                    INNER JOIN Ejemplar e ON m.IdMaterial = e.IdMaterial
                    WHERE e.Activo = 1 AND m.Activo = 1
                    GROUP BY m.IdMaterial, m.Titulo, m.Autor, m.Genero
                    ORDER BY m.Titulo";

                var command = new SqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reportes.Add(new ReporteInventarioEjemplar
                        {
                            IdMaterial = (Guid)reader["IdMaterial"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"]?.ToString() ?? "",
                            Genero = reader["Genero"]?.ToString() ?? "",
                            TotalEjemplares = Convert.ToInt32(reader["TotalEjemplares"]),
                            Disponibles = Convert.ToInt32(reader["Disponibles"]),
                            Prestados = Convert.ToInt32(reader["Prestados"]),
                            Mantenimiento = Convert.ToInt32(reader["Mantenimiento"]),
                            Perdidos = Convert.ToInt32(reader["Perdidos"]),
                            PorcentajeDisponibilidad = Convert.ToDecimal(reader["PorcentajeDisponibilidad"])
                        });
                    }
                }
            }

            return reportes;
        }

        /// <summary>
        /// Obtiene reporte de uso de biblioteca por grado/división
        /// </summary>
        public List<ReporteUsoPorGrado> ObtenerUsoPorGrado(int? anioLectivo = null)
        {
            var reportes = new List<ReporteUsoPorGrado>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    WITH PrestamosPorGrado AS (
                        SELECT
                            i.Grado,
                            i.Division,
                            i.AnioLectivo,
                            COUNT(DISTINCT i.IdAlumno) AS CantidadAlumnos,
                            COUNT(p.IdPrestamo) AS TotalPrestamos,
                            COUNT(CASE WHEN p.Estado = 'Activo' THEN 1 END) AS PrestamosActivos,
                            COUNT(CASE WHEN p.Estado = 'Devuelto' THEN 1 END) AS PrestamosDevueltos,
                            COUNT(CASE WHEN p.Estado = 'Activo' AND GETDATE() > p.FechaDevolucionPrevista THEN 1 END) AS PrestamosVencidos
                        FROM Inscripcion i
                        LEFT JOIN Prestamo p ON i.IdAlumno = p.IdAlumno
                        WHERE i.Estado = 'Activo' ";

                if (anioLectivo.HasValue)
                    query += " AND i.AnioLectivo = @AnioLectivo ";

                query += @"
                        GROUP BY i.Grado, i.Division, i.AnioLectivo
                    ),
                    GenerosPorGrado AS (
                        SELECT
                            i.Grado,
                            i.Division,
                            m.Genero,
                            COUNT(*) AS Cantidad,
                            ROW_NUMBER() OVER (PARTITION BY i.Grado, i.Division ORDER BY COUNT(*) DESC) AS Ranking
                        FROM Inscripcion i
                        INNER JOIN Prestamo p ON i.IdAlumno = p.IdAlumno
                        INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
                        WHERE i.Estado = 'Activo' ";

                if (anioLectivo.HasValue)
                    query += " AND i.AnioLectivo = @AnioLectivo ";

                query += @"
                        GROUP BY i.Grado, i.Division, m.Genero
                    ),
                    MaterialesPorGrado AS (
                        SELECT
                            i.Grado,
                            i.Division,
                            m.Titulo,
                            COUNT(*) AS Cantidad,
                            ROW_NUMBER() OVER (PARTITION BY i.Grado, i.Division ORDER BY COUNT(*) DESC) AS Ranking
                        FROM Inscripcion i
                        INNER JOIN Prestamo p ON i.IdAlumno = p.IdAlumno
                        INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
                        WHERE i.Estado = 'Activo' ";

                if (anioLectivo.HasValue)
                    query += " AND i.AnioLectivo = @AnioLectivo ";

                query += @"
                        GROUP BY i.Grado, i.Division, m.Titulo
                    )
                    SELECT
                        pg.Grado,
                        pg.Division,
                        pg.CantidadAlumnos,
                        ISNULL(pg.TotalPrestamos, 0) AS TotalPrestamos,
                        ISNULL(pg.PrestamosActivos, 0) AS PrestamosActivos,
                        ISNULL(pg.PrestamosDevueltos, 0) AS PrestamosDevueltos,
                        ISNULL(pg.PrestamosVencidos, 0) AS PrestamosVencidos,
                        ISNULL(gp.Genero, 'SIN DATOS') AS GeneroMasPrestado,
                        ISNULL(mp.Titulo, 'SIN DATOS') AS MaterialMasPrestado
                    FROM PrestamosPorGrado pg
                    LEFT JOIN GenerosPorGrado gp ON pg.Grado = gp.Grado AND pg.Division = gp.Division AND gp.Ranking = 1
                    LEFT JOIN MaterialesPorGrado mp ON pg.Grado = mp.Grado AND pg.Division = mp.Division AND mp.Ranking = 1
                    ORDER BY pg.Grado, pg.Division";

                var command = new SqlCommand(query, connection);

                if (anioLectivo.HasValue)
                    command.Parameters.AddWithValue("@AnioLectivo", anioLectivo.Value);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reportes.Add(new ReporteUsoPorGrado
                        {
                            Grado = reader["Grado"].ToString(),
                            Division = reader["Division"].ToString(),
                            CantidadAlumnos = Convert.ToInt32(reader["CantidadAlumnos"]),
                            TotalPrestamos = Convert.ToInt32(reader["TotalPrestamos"]),
                            PrestamosActivos = Convert.ToInt32(reader["PrestamosActivos"]),
                            PrestamosDevueltos = Convert.ToInt32(reader["PrestamosDevueltos"]),
                            PrestamosVencidos = Convert.ToInt32(reader["PrestamosVencidos"]),
                            GeneroMasPrestado = reader["GeneroMasPrestado"].ToString(),
                            MaterialMasPrestado = reader["MaterialMasPrestado"].ToString()
                        });
                    }
                }
            }

            return reportes;
        }
    }
}
