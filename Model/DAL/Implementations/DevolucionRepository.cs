using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    public class DevolucionRepository : IDevolucionRepository
    {
        private readonly string _connectionString;

        public DevolucionRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Add(Devolucion entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Devolucion (IdDevolucion, IdPrestamo, FechaDevolucion, IdUsuario, Observaciones)
                    VALUES (@IdDevolucion, @IdPrestamo, @FechaDevolucion, @IdUsuario, @Observaciones)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdDevolucion", entity.IdDevolucion);
                    cmd.Parameters.AddWithValue("@IdPrestamo", entity.IdPrestamo);
                    cmd.Parameters.AddWithValue("@FechaDevolucion", entity.FechaDevolucion);
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@Observaciones", (object)entity.Observaciones ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Devolucion entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Devolucion
                    SET IdPrestamo = @IdPrestamo,
                        FechaDevolucion = @FechaDevolucion,
                        IdUsuario = @IdUsuario,
                        Observaciones = @Observaciones
                    WHERE IdDevolucion = @IdDevolucion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdDevolucion", entity.IdDevolucion);
                    cmd.Parameters.AddWithValue("@IdPrestamo", entity.IdPrestamo);
                    cmd.Parameters.AddWithValue("@FechaDevolucion", entity.FechaDevolucion);
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@Observaciones", (object)entity.Observaciones ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Devolucion entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Devolucion WHERE IdDevolucion = @IdDevolucion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdDevolucion", entity.IdDevolucion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Devolucion> GetAll()
        {
            List<Devolucion> devoluciones = new List<Devolucion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Devolucion ORDER BY FechaDevolucion DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        devoluciones.Add(DevolucionAdapter.AdaptDevolucion(row));
                    }
                }
            }

            return devoluciones;
        }

        public Devolucion ObtenerPorId(Guid idDevolucion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Devolucion WHERE IdDevolucion = @IdDevolucion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdDevolucion", idDevolucion);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return DevolucionAdapter.AdaptDevolucion(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public Devolucion ObtenerPorPrestamo(Guid idPrestamo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Devolucion WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return DevolucionAdapter.AdaptDevolucion(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public List<Devolucion> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Devolucion> devoluciones = new List<Devolucion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Devolucion
                    WHERE FechaDevolucion >= @FechaInicio
                    AND FechaDevolucion <= @FechaFin
                    ORDER BY FechaDevolucion DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        devoluciones.Add(DevolucionAdapter.AdaptDevolucion(row));
                    }
                }
            }

            return devoluciones;
        }

        public List<Devolucion> ObtenerDevolucionesAtrasadas()
        {
            List<Devolucion> devoluciones = new List<Devolucion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT D.*, P.FechaPrestamo, P.FechaDevolucionPrevista, P.Estado AS EstadoPrestamo
                    FROM Devolucion D
                    INNER JOIN Prestamo P ON D.IdPrestamo = P.IdPrestamo
                    WHERE D.FechaDevolucion > P.FechaDevolucionPrevista
                    ORDER BY D.FechaDevolucion DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        devoluciones.Add(DevolucionAdapter.AdaptDevolucionConPrestamo(row));
                    }
                }
            }

            return devoluciones;
        }
    }
}
