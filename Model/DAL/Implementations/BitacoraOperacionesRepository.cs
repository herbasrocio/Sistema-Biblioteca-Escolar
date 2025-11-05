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
    /// Repositorio para gestionar la bitácora de operaciones en NegocioBiblioteca
    /// </summary>
    public class BitacoraOperacionesRepository : IBitacoraOperacionesRepository
    {
        private readonly string _connectionString;

        public BitacoraOperacionesRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Registrar(BitacoraOperaciones registro)
        {
            string query = @"INSERT INTO BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerTodos()
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerPorTipoOperacion(string tipoOperacion)
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerPorUsuario(Guid idUsuario)
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerPorModulo(string modulo)
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerPorEntidad(string entidadAfectada, int? idEntidad = null)
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones
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

        public List<BitacoraOperaciones> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoOperacion = null, Guid? idUsuario = null, string modulo = null, string entidadAfectada = null)
        {
            List<BitacoraOperaciones> lista = new List<BitacoraOperaciones>();
            List<string> condiciones = new List<string>();

            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoOperacion,
                            Modulo, Accion, EntidadAfectada, IdEntidad, Detalle
                            FROM BitacoraOperaciones WHERE 1=1";

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

        private BitacoraOperaciones MapearDesdeDataReader(SqlDataReader reader)
        {
            return new BitacoraOperaciones
            {
                IdBitacora = reader.GetInt32(reader.GetOrdinal("IdBitacora")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                IdUsuario = reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("IdUsuario")),
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
