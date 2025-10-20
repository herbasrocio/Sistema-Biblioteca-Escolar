using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepository()
        {
            var connStringSetting = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"];
            if (connStringSetting == null)
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión 'NegocioConString' en el archivo de configuración. " +
                    "Asegúrese de que el archivo App.config esté correctamente configurado y copiado al directorio de salida.");
            }
            _connectionString = connStringSetting.ConnectionString;
        }

        public void Add(Material entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Material (
                        IdMaterial, Titulo, Autor, Editorial, Tipo, Genero,
                        ISBN, AnioPublicacion, Nivel,
                        CantidadTotal, CantidadDisponible, FechaRegistro, Activo
                    )
                    VALUES (
                        @IdMaterial, @Titulo, @Autor, @Editorial, @Tipo, @Genero,
                        @ISBN, @AnioPublicacion, @Nivel,
                        @CantidadTotal, @CantidadDisponible, @FechaRegistro, @Activo
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@Titulo", entity.Titulo);
                    cmd.Parameters.AddWithValue("@Autor", entity.Autor);
                    cmd.Parameters.AddWithValue("@Editorial", (object)entity.Editorial ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tipo", entity.Tipo.ToString());
                    cmd.Parameters.AddWithValue("@Genero", (object)entity.Genero ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISBN", (object)entity.ISBN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnioPublicacion", entity.AnioPublicacion.HasValue ? (object)entity.AnioPublicacion.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nivel", (object)entity.Nivel ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CantidadTotal", entity.CantidadTotal);
                    cmd.Parameters.AddWithValue("@CantidadDisponible", entity.CantidadDisponible);
                    cmd.Parameters.AddWithValue("@FechaRegistro", entity.FechaRegistro);
                    cmd.Parameters.AddWithValue("@Activo", entity.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Material entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Material
                    SET Titulo = @Titulo,
                        Autor = @Autor,
                        Editorial = @Editorial,
                        Tipo = @Tipo,
                        Genero = @Genero,
                        ISBN = @ISBN,
                        AnioPublicacion = @AnioPublicacion,
                        Nivel = @Nivel,
                        CantidadTotal = @CantidadTotal,
                        CantidadDisponible = @CantidadDisponible,
                        Activo = @Activo
                    WHERE IdMaterial = @IdMaterial";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@Titulo", entity.Titulo);
                    cmd.Parameters.AddWithValue("@Autor", entity.Autor);
                    cmd.Parameters.AddWithValue("@Editorial", (object)entity.Editorial ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tipo", entity.Tipo.ToString());
                    cmd.Parameters.AddWithValue("@Genero", (object)entity.Genero ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISBN", (object)entity.ISBN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnioPublicacion", entity.AnioPublicacion.HasValue ? (object)entity.AnioPublicacion.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nivel", (object)entity.Nivel ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CantidadTotal", entity.CantidadTotal);
                    cmd.Parameters.AddWithValue("@CantidadDisponible", entity.CantidadDisponible);
                    cmd.Parameters.AddWithValue("@Activo", entity.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Material entity)
        {
            // Borrado lógico
            entity.Activo = false;
            Update(entity);
        }

        public List<Material> GetAll()
        {
            List<Material> materiales = new List<Material>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Consulta que calcula dinámicamente las cantidades basándose en los ejemplares reales
                // NOTA: EstadoMaterial.Disponible = 0 (primer valor del enum)
                string query = @"
                    SELECT
                        m.*,
                        ISNULL((SELECT COUNT(*)
                                FROM Ejemplar e
                                WHERE e.IdMaterial = m.IdMaterial
                                AND e.Activo = 1), 0) AS CantidadTotalCalculada,
                        ISNULL((SELECT COUNT(*)
                                FROM Ejemplar e
                                WHERE e.IdMaterial = m.IdMaterial
                                AND e.Activo = 1
                                AND e.Estado = 0), 0) AS CantidadDisponibleCalculada
                    FROM Material m
                    WHERE m.Activo = 1
                    ORDER BY m.Titulo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Material material = MaterialAdapter.AdaptMaterial(row);

                        // Sobrescribir con los valores calculados
                        material.CantidadTotal = Convert.ToInt32(row["CantidadTotalCalculada"]);
                        material.CantidadDisponible = Convert.ToInt32(row["CantidadDisponibleCalculada"]);

                        materiales.Add(material);
                    }
                }
            }

            return materiales;
        }

        public Material ObtenerPorId(Guid idMaterial)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // NOTA: EstadoMaterial.Disponible = 0 (primer valor del enum)
                string query = @"
                    SELECT
                        m.*,
                        ISNULL((SELECT COUNT(*)
                                FROM Ejemplar e
                                WHERE e.IdMaterial = m.IdMaterial
                                AND e.Activo = 1), 0) AS CantidadTotalCalculada,
                        ISNULL((SELECT COUNT(*)
                                FROM Ejemplar e
                                WHERE e.IdMaterial = m.IdMaterial
                                AND e.Activo = 1
                                AND e.Estado = 0), 0) AS CantidadDisponibleCalculada
                    FROM Material m
                    WHERE m.IdMaterial = @IdMaterial";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", idMaterial);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Material material = MaterialAdapter.AdaptMaterial(dt.Rows[0]);

                        // Sobrescribir con los valores calculados
                        material.CantidadTotal = Convert.ToInt32(dt.Rows[0]["CantidadTotalCalculada"]);
                        material.CantidadDisponible = Convert.ToInt32(dt.Rows[0]["CantidadDisponibleCalculada"]);

                        return material;
                    }
                }
            }

            return null;
        }

        public List<Material> BuscarPorFiltros(string titulo, string autor, string tipo)
        {
            List<Material> materiales = new List<Material>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // NOTA: EstadoMaterial.Disponible = 0 (primer valor del enum)
                string query = @"
                    SELECT
                        m.*,
                        ISNULL((SELECT COUNT(*)
                                FROM Ejemplar e
                                WHERE e.IdMaterial = m.IdMaterial
                                AND e.Activo = 1), 0) AS CantidadTotalCalculada,
                        ISNULL((SELECT COUNT(*)
                                FROM Ejemplar e
                                WHERE e.IdMaterial = m.IdMaterial
                                AND e.Activo = 1
                                AND e.Estado = 0), 0) AS CantidadDisponibleCalculada
                    FROM Material m
                    WHERE m.Activo = 1
                    AND (@Titulo IS NULL OR m.Titulo LIKE '%' + @Titulo + '%')
                    AND (@Autor IS NULL OR m.Autor LIKE '%' + @Autor + '%')
                    AND (@Tipo IS NULL OR @Tipo = 'Todos' OR m.Tipo = @Tipo)
                    ORDER BY m.Titulo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Titulo", string.IsNullOrWhiteSpace(titulo) ? (object)DBNull.Value : titulo);
                    cmd.Parameters.AddWithValue("@Autor", string.IsNullOrWhiteSpace(autor) ? (object)DBNull.Value : autor);
                    cmd.Parameters.AddWithValue("@Tipo", string.IsNullOrWhiteSpace(tipo) ? (object)DBNull.Value : tipo);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Material material = MaterialAdapter.AdaptMaterial(row);

                        // Sobrescribir con los valores calculados
                        material.CantidadTotal = Convert.ToInt32(row["CantidadTotalCalculada"]);
                        material.CantidadDisponible = Convert.ToInt32(row["CantidadDisponibleCalculada"]);

                        materiales.Add(material);
                    }
                }
            }

            return materiales;
        }
    }
}
