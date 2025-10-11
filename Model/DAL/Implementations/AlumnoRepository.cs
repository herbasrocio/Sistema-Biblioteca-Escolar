using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly string _connectionString;

        public AlumnoRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Add(Alumno entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Alumno (IdAlumno, Nombre, Apellido, DNI, Grado, Division, FechaRegistro)
                    VALUES (@IdAlumno, @Nombre, @Apellido, @DNI, @Grado, @Division, @FechaRegistro)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", entity.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", entity.DNI);
                    cmd.Parameters.AddWithValue("@Grado", (object)entity.Grado ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Division", (object)entity.Division ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaRegistro", entity.FechaRegistro);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Alumno entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Alumno
                    SET Nombre = @Nombre,
                        Apellido = @Apellido,
                        DNI = @DNI,
                        Grado = @Grado,
                        Division = @Division
                    WHERE IdAlumno = @IdAlumno";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", entity.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", entity.DNI);
                    cmd.Parameters.AddWithValue("@Grado", (object)entity.Grado ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Division", (object)entity.Division ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Alumno entity)
        {
            // Eliminación física del alumno
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Alumno WHERE IdAlumno = @IdAlumno";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Alumno> GetAll()
        {
            List<Alumno> alumnos = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Alumno ORDER BY Apellido, Nombre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        alumnos.Add(AlumnoAdapter.AdaptAlumno(row));
                    }
                }
            }

            return alumnos;
        }

        public Alumno ObtenerPorId(Guid idAlumno)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Alumno WHERE IdAlumno = @IdAlumno";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return AlumnoAdapter.AdaptAlumno(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public Alumno ObtenerPorDNI(string dni)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Alumno WHERE DNI = @DNI";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return AlumnoAdapter.AdaptAlumno(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public List<Alumno> BuscarPorNombre(string nombre)
        {
            List<Alumno> alumnos = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Alumno
                    WHERE (Nombre LIKE '%' + @Nombre + '%' OR Apellido LIKE '%' + @Nombre + '%')
                    ORDER BY Apellido, Nombre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        alumnos.Add(AlumnoAdapter.AdaptAlumno(row));
                    }
                }
            }

            return alumnos;
        }

        public List<Alumno> BuscarPorGradoDivision(string grado, string division)
        {
            List<Alumno> alumnos = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Alumno
                    WHERE (@Grado IS NULL OR Grado = @Grado)
                    AND (@Division IS NULL OR Division = @Division)
                    ORDER BY Apellido, Nombre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Grado", string.IsNullOrWhiteSpace(grado) ? (object)DBNull.Value : grado);
                    cmd.Parameters.AddWithValue("@Division", string.IsNullOrWhiteSpace(division) ? (object)DBNull.Value : division);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        alumnos.Add(AlumnoAdapter.AdaptAlumno(row));
                    }
                }
            }

            return alumnos;
        }
    }
}
