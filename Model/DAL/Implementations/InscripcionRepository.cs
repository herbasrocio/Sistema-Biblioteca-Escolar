using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    /// <summary>
    /// Repositorio para gestión de Inscripciones
    /// </summary>
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly string _connectionString;

        public InscripcionRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Add(Inscripcion entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Inscripcion (IdInscripcion, IdAlumno, AnioLectivo, Grado, Division, FechaInscripcion, Estado)
                    VALUES (@IdInscripcion, @IdAlumno, @AnioLectivo, @Grado, @Division, @FechaInscripcion, @Estado)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInscripcion", entity.IdInscripcion);
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@AnioLectivo", entity.AnioLectivo);
                    cmd.Parameters.AddWithValue("@Grado", entity.Grado);
                    cmd.Parameters.AddWithValue("@Division", (object)entity.Division ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInscripcion", entity.FechaInscripcion);
                    cmd.Parameters.AddWithValue("@Estado", entity.Estado);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Inscripcion entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Inscripcion
                    SET Grado = @Grado,
                        Division = @Division,
                        Estado = @Estado
                    WHERE IdInscripcion = @IdInscripcion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInscripcion", entity.IdInscripcion);
                    cmd.Parameters.AddWithValue("@Grado", entity.Grado);
                    cmd.Parameters.AddWithValue("@Division", (object)entity.Division ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", entity.Estado);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Inscripcion entity)
        {
            // Eliminación lógica: cambiar estado a Abandonado
            entity.Estado = "Abandonado";
            Update(entity);
        }

        public List<Inscripcion> GetAll()
        {
            List<Inscripcion> inscripciones = new List<Inscripcion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Inscripcion
                    WHERE Estado <> 'Abandonado'
                    ORDER BY AnioLectivo DESC, Grado, Division";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        inscripciones.Add(InscripcionAdapter.AdaptInscripcion(row));
                    }
                }
            }

            return inscripciones;
        }

        public Inscripcion ObtenerPorId(Guid idInscripcion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Inscripcion WHERE IdInscripcion = @IdInscripcion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInscripcion", idInscripcion);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return InscripcionAdapter.AdaptInscripcion(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public Inscripcion ObtenerInscripcionActiva(Guid idAlumno, int anioLectivo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Inscripcion
                    WHERE IdAlumno = @IdAlumno
                        AND AnioLectivo = @AnioLectivo
                        AND Estado = 'Activo'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@AnioLectivo", anioLectivo);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return InscripcionAdapter.AdaptInscripcion(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public List<Inscripcion> ObtenerPorAlumno(Guid idAlumno)
        {
            List<Inscripcion> inscripciones = new List<Inscripcion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Inscripcion
                    WHERE IdAlumno = @IdAlumno
                    ORDER BY AnioLectivo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        inscripciones.Add(InscripcionAdapter.AdaptInscripcion(row));
                    }
                }
            }

            return inscripciones;
        }

        public List<Inscripcion> ObtenerPorGradoDivision(int anioLectivo, string grado, string division = null)
        {
            List<Inscripcion> inscripciones = new List<Inscripcion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Inscripcion
                    WHERE AnioLectivo = @AnioLectivo
                        AND Grado = @Grado
                        AND (@Division IS NULL OR Division = @Division)
                        AND Estado = 'Activo'
                    ORDER BY Division";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AnioLectivo", anioLectivo);
                    cmd.Parameters.AddWithValue("@Grado", grado);
                    cmd.Parameters.AddWithValue("@Division", string.IsNullOrWhiteSpace(division) ? (object)DBNull.Value : division);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        inscripciones.Add(InscripcionAdapter.AdaptInscripcion(row));
                    }
                }
            }

            return inscripciones;
        }

        public List<Inscripcion> ObtenerPorAnioLectivo(int anioLectivo)
        {
            List<Inscripcion> inscripciones = new List<Inscripcion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Inscripcion
                    WHERE AnioLectivo = @AnioLectivo
                    ORDER BY Grado, Division";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AnioLectivo", anioLectivo);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        inscripciones.Add(InscripcionAdapter.AdaptInscripcion(row));
                    }
                }
            }

            return inscripciones;
        }

        public void FinalizarInscripcion(Guid idInscripcion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Inscripcion
                    SET Estado = 'Finalizado'
                    WHERE IdInscripcion = @IdInscripcion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInscripcion", idInscripcion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int FinalizarInscripcionesPorAnio(int anioLectivo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Inscripcion
                    SET Estado = 'Finalizado'
                    WHERE AnioLectivo = @AnioLectivo
                        AND Estado = 'Activo'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AnioLectivo", anioLectivo);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
