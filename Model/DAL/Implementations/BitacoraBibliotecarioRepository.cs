using DAL.Contracts;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementations
{
    /// <summary>
    /// Repositorio para gestionar la bitácora de bibliotecario en NegocioBiblioteca
    /// </summary>
    public class BitacoraBibliotecarioRepository : IBitacoraBibliotecarioRepository
    {
        private readonly string _connectionString;

        public BitacoraBibliotecarioRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Registrar(BitacoraBibliotecario registro)
        {
            string query = @"INSERT INTO BitacoraBibliotecario
                (Fecha, IdUsuario, NombreUsuario, TipoOperacion, Modulo, Accion, EntidadAfectada, IdEntidad, Detalle)
                VALUES
                (@Fecha, @IdUsuario, @NombreUsuario, @TipoOperacion, @Modulo, @Accion, @EntidadAfectada, @IdEntidad, @Detalle)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Fecha", registro.Fecha);
                    cmd.Parameters.AddWithValue("@IdUsuario", (object)registro.IdUsuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreUsuario", (object)registro.NombreUsuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TipoOperacion", registro.TipoOperacion);
                    cmd.Parameters.AddWithValue("@Modulo", registro.Modulo);
                    cmd.Parameters.AddWithValue("@Accion", registro.Accion);
                    cmd.Parameters.AddWithValue("@EntidadAfectada", (object)registro.EntidadAfectada ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdEntidad", (object)registro.IdEntidad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Detalle", (object)registro.Detalle ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<BitacoraBibliotecario> ObtenerTodos()
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<BitacoraBibliotecario> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario
                            WHERE Fecha BETWEEN @FechaInicio AND @FechaFin
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<BitacoraBibliotecario> ObtenerPorTipoOperacion(string tipoOperacion)
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario
                            WHERE TipoOperacion = @TipoOperacion
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TipoOperacion", tipoOperacion);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<BitacoraBibliotecario> ObtenerPorUsuario(Guid idUsuario)
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario
                            WHERE IdUsuario = @IdUsuario
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<BitacoraBibliotecario> ObtenerPorModulo(string modulo)
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario
                            WHERE Modulo = @Modulo
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Modulo", modulo);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<BitacoraBibliotecario> ObtenerPorEntidad(string entidadAfectada, int? idEntidad = null)
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario
                            WHERE EntidadAfectada = @EntidadAfectada";

            if (idEntidad.HasValue)
                query += " AND IdEntidad = @IdEntidad";

            query += " ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EntidadAfectada", entidadAfectada);

                    if (idEntidad.HasValue)
                        cmd.Parameters.AddWithValue("@IdEntidad", idEntidad.Value);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<BitacoraBibliotecario> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoOperacion = null, Guid? idUsuario = null, string modulo = null, string entidadAfectada = null)
        {
            List<BitacoraBibliotecario> lista = new List<BitacoraBibliotecario>();
            List<string> condiciones = new List<string>();

            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraBibliotecario WHERE 1=1";

            if (fechaInicio.HasValue)
                condiciones.Add("Fecha >= @FechaInicio");

            if (fechaFin.HasValue)
                condiciones.Add("Fecha <= @FechaFin");

            if (!string.IsNullOrWhiteSpace(tipoOperacion))
                condiciones.Add("TipoOperacion = @TipoOperacion");

            if (idUsuario.HasValue)
                condiciones.Add("IdUsuario = @IdUsuario");

            if (!string.IsNullOrWhiteSpace(modulo))
                condiciones.Add("Modulo = @Modulo");

            if (!string.IsNullOrWhiteSpace(entidadAfectada))
                condiciones.Add("EntidadAfectada = @EntidadAfectada");

            if (condiciones.Count > 0)
                query += " AND " + string.Join(" AND ", condiciones);

            query += " ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (fechaInicio.HasValue)
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Value);

                    if (fechaFin.HasValue)
                        cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Value);

                    if (!string.IsNullOrWhiteSpace(tipoOperacion))
                        cmd.Parameters.AddWithValue("@TipoOperacion", tipoOperacion);

                    if (idUsuario.HasValue)
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario.Value);

                    if (!string.IsNullOrWhiteSpace(modulo))
                        cmd.Parameters.AddWithValue("@Modulo", modulo);

                    if (!string.IsNullOrWhiteSpace(entidadAfectada))
                        cmd.Parameters.AddWithValue("@EntidadAfectada", entidadAfectada);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(MapearDesdeDataReader(reader));
                        }
                    }
                }
            }

            return lista;
        }

        private BitacoraBibliotecario MapearDesdeDataReader(SqlDataReader reader)
        {
            return new BitacoraBibliotecario
            {
                IdBitacora = reader.GetInt32(reader.GetOrdinal("IdBitacora")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                IdUsuario = reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                TipoOperacion = reader.GetString(reader.GetOrdinal("TipoOperacion")),
                Modulo = reader.GetString(reader.GetOrdinal("Modulo")),
                Accion = reader.GetString(reader.GetOrdinal("Accion")),
                EntidadAfectada = reader.IsDBNull(reader.GetOrdinal("EntidadAfectada")) ? null : reader.GetString(reader.GetOrdinal("EntidadAfectada")),
                IdEntidad = reader.IsDBNull(reader.GetOrdinal("IdEntidad")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdEntidad")),
                Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString(reader.GetOrdinal("Detalle"))
            };
        }
    }
}
