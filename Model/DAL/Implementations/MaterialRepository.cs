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
    /// Implementación del repositorio para el acceso a datos de materiales bibliográficos.
    /// Proporciona operaciones CRUD y búsquedas especializadas contra la base de datos SQL Server.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Esta clase implementa el patrón Repository para la entidad <see cref="Material"/>,
    /// encapsulando toda la lógica de acceso a datos y consultas SQL.
    /// </para>
    /// <para>
    /// Las cantidades de ejemplares (CantidadTotal y CantidadDisponible) se calculan dinámicamente
    /// mediante subconsultas SQL basándose en los ejemplares activos y su estado.
    /// </para>
    /// </remarks>
    public class MaterialRepository : IMaterialRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MaterialRepository"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Se lanza si no se encuentra la cadena de conexión 'NegocioConString' en el archivo de configuración.
        /// </exception>
        /// <remarks>
        /// La cadena de conexión se obtiene del archivo App.config mediante ConfigurationManager.
        /// Es importante que el archivo App.config esté correctamente configurado y copiado al directorio de salida.
        /// </remarks>
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

        /// <summary>
        /// Agrega un nuevo material al catálogo de la biblioteca.
        /// </summary>
        /// <param name="entity">Material a insertar en la base de datos.</param>
        /// <remarks>
        /// Inserta todos los campos del material incluyendo IdMaterial (GUID), información bibliográfica,
        /// cantidades y estado de activación. Los valores nulos se manejan correctamente usando DBNull.Value.
        /// </remarks>
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

        /// <summary>
        /// Actualiza un material existente en el catálogo.
        /// </summary>
        /// <param name="entity">Material con los datos actualizados.</param>
        /// <remarks>
        /// Actualiza todos los campos excepto IdMaterial y FechaRegistro.
        /// Los valores nulos se manejan correctamente usando DBNull.Value.
        /// </remarks>
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

        /// <summary>
        /// Elimina lógicamente un material del catálogo.
        /// </summary>
        /// <param name="entity">Material a eliminar.</param>
        /// <remarks>
        /// Implementa borrado lógico (soft delete) marcando el material como inactivo.
        /// No se elimina físicamente de la base de datos.
        /// </remarks>
        public void Delete(Material entity)
        {
            // Borrado lógico
            entity.Activo = false;
            Update(entity);
        }

        /// <summary>
        /// Obtiene todos los materiales activos del catálogo con cantidades calculadas dinámicamente.
        /// </summary>
        /// <returns>
        /// Lista de todos los materiales activos ordenados por título.
        /// Las propiedades CantidadTotal y CantidadDisponible se calculan en tiempo real.
        /// </returns>
        /// <remarks>
        /// <para>
        /// La consulta SQL calcula dinámicamente:
        /// <list type="bullet">
        /// <item><description>CantidadTotal: Cuenta ejemplares activos asociados al material.</description></item>
        /// <item><description>CantidadDisponible: Cuenta ejemplares activos con estado Disponible (valor 0).</description></item>
        /// </list>
        /// </para>
        /// <para>Solo devuelve materiales con Activo=true.</para>
        /// </remarks>
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

        /// <summary>
        /// Obtiene un material específico por su identificador con cantidades calculadas dinámicamente.
        /// </summary>
        /// <param name="idMaterial">Identificador único del material.</param>
        /// <returns>
        /// El objeto <see cref="Material"/> con cantidades actualizadas dinámicamente,
        /// o <c>null</c> si no se encuentra.
        /// </returns>
        /// <remarks>
        /// Similar a <see cref="GetAll"/>, las cantidades se calculan en tiempo real
        /// basándose en los ejemplares activos asociados.
        /// </remarks>
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

        /// <summary>
        /// Busca materiales aplicando filtros opcionales con cantidades calculadas dinámicamente.
        /// </summary>
        /// <param name="titulo">Título o parte del título. <c>null</c> o vacío para omitir filtro.</param>
        /// <param name="autor">Autor o parte del autor. <c>null</c> o vacío para omitir filtro.</param>
        /// <param name="tipo">Tipo de material. <c>null</c>, vacío o "Todos" para omitir filtro.</param>
        /// <returns>
        /// Lista de materiales que coinciden con los criterios, ordenados por título.
        /// Las cantidades se calculan dinámicamente.
        /// </returns>
        /// <remarks>
        /// <para>
        /// La búsqueda utiliza operador LIKE para coincidencias parciales en título y autor.
        /// Los filtros se combinan con operador AND. La búsqueda es insensible a mayúsculas/minúsculas.
        /// </para>
        /// <para>Solo devuelve materiales activos.</para>
        /// </remarks>
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
