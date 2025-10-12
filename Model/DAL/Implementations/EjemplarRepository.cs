using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;
using DomainModel.Enums;

namespace DAL.Implementations
{
    public class EjemplarRepository : IEjemplarRepository
    {
        private readonly string _connectionString;

        public EjemplarRepository()
        {
            var connStringSetting = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"];
            if (connStringSetting == null)
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión 'NegocioConString' en el archivo de configuración.");
            }
            _connectionString = connStringSetting.ConnectionString;
        }

        public void Add(Ejemplar entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Ejemplar (
                        IdEjemplar, IdMaterial, NumeroEjemplar, CodigoBarras,
                        Estado, Ubicacion, Observaciones, FechaRegistro, Activo
                    )
                    VALUES (
                        @IdEjemplar, @IdMaterial, @NumeroEjemplar, @CodigoBarras,
                        @Estado, @Ubicacion, @Observaciones, @FechaRegistro, @Activo
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdEjemplar", entity.IdEjemplar);
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@NumeroEjemplar", entity.NumeroEjemplar);
                    cmd.Parameters.AddWithValue("@CodigoBarras", (object)entity.CodigoBarras ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", (int)entity.Estado);
                    cmd.Parameters.AddWithValue("@Ubicacion", (object)entity.Ubicacion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Observaciones", (object)entity.Observaciones ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaRegistro", entity.FechaRegistro);
                    cmd.Parameters.AddWithValue("@Activo", entity.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Ejemplar entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Ejemplar
                    SET NumeroEjemplar = @NumeroEjemplar,
                        CodigoBarras = @CodigoBarras,
                        Estado = @Estado,
                        Ubicacion = @Ubicacion,
                        Observaciones = @Observaciones,
                        Activo = @Activo
                    WHERE IdEjemplar = @IdEjemplar";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdEjemplar", entity.IdEjemplar);
                    cmd.Parameters.AddWithValue("@NumeroEjemplar", entity.NumeroEjemplar);
                    cmd.Parameters.AddWithValue("@CodigoBarras", (object)entity.CodigoBarras ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", (int)entity.Estado);
                    cmd.Parameters.AddWithValue("@Ubicacion", (object)entity.Ubicacion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Observaciones", (object)entity.Observaciones ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activo", entity.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Ejemplar entity)
        {
            // Borrado lógico
            entity.Activo = false;
            Update(entity);
        }

        public List<Ejemplar> GetAll()
        {
            List<Ejemplar> ejemplares = new List<Ejemplar>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Ejemplar WHERE Activo = 1 ORDER BY IdMaterial, NumeroEjemplar";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        ejemplares.Add(EjemplarAdapter.AdaptEjemplar(row));
                    }
                }
            }

            return ejemplares;
        }

        public List<Ejemplar> ObtenerPorMaterial(Guid idMaterial)
        {
            List<Ejemplar> ejemplares = new List<Ejemplar>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Ejemplar
                    WHERE IdMaterial = @IdMaterial AND Activo = 1
                    ORDER BY NumeroEjemplar";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", idMaterial);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        ejemplares.Add(EjemplarAdapter.AdaptEjemplar(row));
                    }
                }
            }

            return ejemplares;
        }

        public Ejemplar ObtenerPorId(Guid idEjemplar)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Ejemplar WHERE IdEjemplar = @IdEjemplar";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdEjemplar", idEjemplar);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return EjemplarAdapter.AdaptEjemplar(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public List<Ejemplar> ObtenerPorEstado(EstadoMaterial estado)
        {
            List<Ejemplar> ejemplares = new List<Ejemplar>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Ejemplar
                    WHERE Estado = @Estado AND Activo = 1
                    ORDER BY IdMaterial, NumeroEjemplar";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Estado", (int)estado);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        ejemplares.Add(EjemplarAdapter.AdaptEjemplar(row));
                    }
                }
            }

            return ejemplares;
        }

        public Ejemplar ObtenerPorCodigoBarras(string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                return null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Ejemplar WHERE CodigoBarras = @CodigoBarras AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CodigoBarras", codigoBarras);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return EjemplarAdapter.AdaptEjemplar(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public int ContarDisponiblesPorMaterial(Guid idMaterial)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT COUNT(*)
                    FROM Ejemplar
                    WHERE IdMaterial = @IdMaterial
                      AND Estado = @EstadoDisponible
                      AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", idMaterial);
                    cmd.Parameters.AddWithValue("@EstadoDisponible", (int)EstadoMaterial.Disponible);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void ActualizarEstado(Guid idEjemplar, EstadoMaterial nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Ejemplar
                    SET Estado = @Estado
                    WHERE IdEjemplar = @IdEjemplar";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdEjemplar", idEjemplar);
                    cmd.Parameters.AddWithValue("@Estado", (int)nuevoEstado);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
