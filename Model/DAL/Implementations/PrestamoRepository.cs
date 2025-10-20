using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly string _connectionString;

        public PrestamoRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Add(Prestamo entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Prestamo (IdPrestamo, IdMaterial, IdEjemplar, IdAlumno, IdUsuario, FechaPrestamo, FechaDevolucionPrevista, Estado)
                    VALUES (@IdPrestamo, @IdMaterial, @IdEjemplar, @IdAlumno, @IdUsuario, @FechaPrestamo, @FechaDevolucionPrevista, @Estado)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", entity.IdPrestamo);
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@IdEjemplar", entity.IdEjemplar);
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@FechaPrestamo", entity.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@FechaDevolucionPrevista", entity.FechaDevolucionPrevista);
                    cmd.Parameters.AddWithValue("@Estado", entity.Estado);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Prestamo entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Prestamo
                    SET IdMaterial = @IdMaterial,
                        IdEjemplar = @IdEjemplar,
                        IdAlumno = @IdAlumno,
                        IdUsuario = @IdUsuario,
                        FechaPrestamo = @FechaPrestamo,
                        FechaDevolucionPrevista = @FechaDevolucionPrevista,
                        Estado = @Estado
                    WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", entity.IdPrestamo);
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@IdEjemplar", entity.IdEjemplar);
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@FechaPrestamo", entity.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@FechaDevolucionPrevista", entity.FechaDevolucionPrevista);
                    cmd.Parameters.AddWithValue("@Estado", entity.Estado);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Prestamo entity)
        {
            // No se borran físicamente, se cambia el estado
            entity.Estado = "Cancelado";
            Update(entity);
        }

        public List<Prestamo> GetAll()
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Prestamo ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public Prestamo ObtenerPorId(Guid idPrestamo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Prestamo WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return PrestamoAdapter.AdaptPrestamo(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public List<Prestamo> ObtenerPorAlumno(Guid idAlumno)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE IdAlumno = @IdAlumno
                    ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public List<Prestamo> ObtenerPorMaterial(Guid idMaterial)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE IdMaterial = @IdMaterial
                    ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", idMaterial);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public List<Prestamo> ObtenerActivos()
        {
            return ObtenerPorEstado("Activo");
        }

        public List<Prestamo> ObtenerAtrasados()
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE Estado = 'Activo'
                    AND FechaDevolucionPrevista < GETDATE()
                    ORDER BY FechaDevolucionPrevista";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public List<Prestamo> ObtenerPorEstado(string estado)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE Estado = @Estado
                    ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public void ActualizarEstado(Guid idPrestamo, string nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Prestamo
                    SET Estado = @Estado
                    WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                    cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
