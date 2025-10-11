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
                    INSERT INTO Material (IdMaterial, Titulo, Autor, Editorial, Tipo, Genero, CantidadTotal, CantidadDisponible, FechaRegistro, Activo)
                    VALUES (@IdMaterial, @Titulo, @Autor, @Editorial, @Tipo, @Genero, @CantidadTotal, @CantidadDisponible, @FechaRegistro, @Activo)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@Titulo", entity.Titulo);
                    cmd.Parameters.AddWithValue("@Autor", entity.Autor);
                    cmd.Parameters.AddWithValue("@Editorial", (object)entity.Editorial ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tipo", entity.Tipo);
                    cmd.Parameters.AddWithValue("@Genero", entity.Genero);
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
                    cmd.Parameters.AddWithValue("@Tipo", entity.Tipo);
                    cmd.Parameters.AddWithValue("@Genero", entity.Genero);
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
                string query = "SELECT * FROM Material WHERE Activo = 1 ORDER BY Titulo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        materiales.Add(MaterialAdapter.AdaptMaterial(row));
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
                string query = "SELECT * FROM Material WHERE IdMaterial = @IdMaterial";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", idMaterial);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return MaterialAdapter.AdaptMaterial(dt.Rows[0]);
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
                string query = @"
                    SELECT * FROM Material
                    WHERE Activo = 1
                    AND (@Titulo IS NULL OR Titulo LIKE '%' + @Titulo + '%')
                    AND (@Autor IS NULL OR Autor LIKE '%' + @Autor + '%')
                    AND (@Tipo IS NULL OR @Tipo = 'Todos' OR Tipo = @Tipo)
                    ORDER BY Titulo";

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
                        materiales.Add(MaterialAdapter.AdaptMaterial(row));
                    }
                }
            }

            return materiales;
        }
    }
}
