using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    public class HistorialEstadoEjemplarRepository : IHistorialEstadoEjemplarRepository
    {
        private readonly string _connectionString;

        public HistorialEstadoEjemplarRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void RegistrarCambio(HistorialEstadoEjemplar historial)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO HistorialEstadoEjemplar (
                        IdHistorial,
                        IdEjemplar,
                        EstadoAnterior,
                        EstadoNuevo,
                        FechaCambio,
                        IdUsuario,
                        Motivo,
                        IdPrestamo,
                        IdDevolucion,
                        TipoCambio
                    ) VALUES (
                        @IdHistorial,
                        @IdEjemplar,
                        @EstadoAnterior,
                        @EstadoNuevo,
                        @FechaCambio,
                        @IdUsuario,
                        @Motivo,
                        @IdPrestamo,
                        @IdDevolucion,
                        @TipoCambio
                    )";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdHistorial", historial.IdHistorial);
                    command.Parameters.AddWithValue("@IdEjemplar", historial.IdEjemplar);
                    command.Parameters.AddWithValue("@EstadoAnterior", (int)historial.EstadoAnterior);
                    command.Parameters.AddWithValue("@EstadoNuevo", (int)historial.EstadoNuevo);
                    command.Parameters.AddWithValue("@FechaCambio", historial.FechaCambio);
                    command.Parameters.AddWithValue("@IdUsuario", (object)historial.IdUsuario ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Motivo", (object)historial.Motivo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdPrestamo", (object)historial.IdPrestamo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdDevolucion", (object)historial.IdDevolucion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TipoCambio", HistorialEstadoEjemplarAdapter.TipoCambioToString(historial.TipoCambio));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<HistorialEstadoEjemplar> ObtenerHistorialPorEjemplar(Guid idEjemplar)
        {
            List<HistorialEstadoEjemplar> historial = new List<HistorialEstadoEjemplar>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        h.IdHistorial,
                        h.IdEjemplar,
                        h.EstadoAnterior,
                        h.EstadoNuevo,
                        h.FechaCambio,
                        h.IdUsuario,
                        h.Motivo,
                        h.IdPrestamo,
                        h.IdDevolucion,
                        h.TipoCambio
                    FROM HistorialEstadoEjemplar h
                    WHERE h.IdEjemplar = @IdEjemplar
                    ORDER BY h.FechaCambio DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdEjemplar", idEjemplar);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            foreach (DataRow row in dt.Rows)
                            {
                                historial.Add(HistorialEstadoEjemplarAdapter.AdaptHistorial(row));
                            }
                            break;
                        }
                    }
                }
            }

            return historial;
        }

        public List<HistorialEstadoEjemplar> ObtenerHistorialPorTipo(Guid idEjemplar, TipoCambioEstado tipoCambio)
        {
            List<HistorialEstadoEjemplar> historial = new List<HistorialEstadoEjemplar>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        h.IdHistorial,
                        h.IdEjemplar,
                        h.EstadoAnterior,
                        h.EstadoNuevo,
                        h.FechaCambio,
                        h.IdUsuario,
                        h.Motivo,
                        h.IdPrestamo,
                        h.IdDevolucion,
                        h.TipoCambio
                    FROM HistorialEstadoEjemplar h
                    WHERE h.IdEjemplar = @IdEjemplar
                      AND h.TipoCambio = @TipoCambio
                    ORDER BY h.FechaCambio DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdEjemplar", idEjemplar);
                    command.Parameters.AddWithValue("@TipoCambio", HistorialEstadoEjemplarAdapter.TipoCambioToString(tipoCambio));

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            foreach (DataRow row in dt.Rows)
                            {
                                historial.Add(HistorialEstadoEjemplarAdapter.AdaptHistorial(row));
                            }
                            break;
                        }
                    }
                }
            }

            return historial;
        }

        public List<HistorialEstadoEjemplar> ObtenerHistorialPorFechas(Guid idEjemplar, DateTime fechaInicio, DateTime fechaFin)
        {
            List<HistorialEstadoEjemplar> historial = new List<HistorialEstadoEjemplar>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        h.IdHistorial,
                        h.IdEjemplar,
                        h.EstadoAnterior,
                        h.EstadoNuevo,
                        h.FechaCambio,
                        h.IdUsuario,
                        h.Motivo,
                        h.IdPrestamo,
                        h.IdDevolucion,
                        h.TipoCambio
                    FROM HistorialEstadoEjemplar h
                    WHERE h.IdEjemplar = @IdEjemplar
                      AND h.FechaCambio BETWEEN @FechaInicio AND @FechaFin
                    ORDER BY h.FechaCambio DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdEjemplar", idEjemplar);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            foreach (DataRow row in dt.Rows)
                            {
                                historial.Add(HistorialEstadoEjemplarAdapter.AdaptHistorial(row));
                            }
                            break;
                        }
                    }
                }
            }

            return historial;
        }

        public HistorialEstadoEjemplar ObtenerUltimoCambio(Guid idEjemplar)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT TOP 1
                        h.IdHistorial,
                        h.IdEjemplar,
                        h.EstadoAnterior,
                        h.EstadoNuevo,
                        h.FechaCambio,
                        h.IdUsuario,
                        h.Motivo,
                        h.IdPrestamo,
                        h.IdDevolucion,
                        h.TipoCambio
                    FROM HistorialEstadoEjemplar h
                    WHERE h.IdEjemplar = @IdEjemplar
                    ORDER BY h.FechaCambio DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdEjemplar", idEjemplar);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            if (dt.Rows.Count > 0)
                            {
                                return HistorialEstadoEjemplarAdapter.AdaptHistorial(dt.Rows[0]);
                            }
                        }
                    }
                }
            }

            return null;
        }

        public List<HistorialEstadoEjemplar> ObtenerTodos()
        {
            List<HistorialEstadoEjemplar> historial = new List<HistorialEstadoEjemplar>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        h.IdHistorial,
                        h.IdEjemplar,
                        h.EstadoAnterior,
                        h.EstadoNuevo,
                        h.FechaCambio,
                        h.IdUsuario,
                        h.Motivo,
                        h.IdPrestamo,
                        h.IdDevolucion,
                        h.TipoCambio
                    FROM HistorialEstadoEjemplar h
                    ORDER BY h.FechaCambio DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            foreach (DataRow row in dt.Rows)
                            {
                                historial.Add(HistorialEstadoEjemplarAdapter.AdaptHistorial(row));
                            }
                            break;
                        }
                    }
                }
            }

            return historial;
        }
    }
}
