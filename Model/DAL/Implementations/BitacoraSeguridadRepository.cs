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
    /// Repositorio para gestionar la bitácora de administrador en SeguridadBiblioteca
    /// </summary>
    public class BitacoraSeguridadRepository : IBitacoraSeguridadRepository
    {
        private readonly string _connectionString;

        public BitacoraSeguridadRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ServicesConString"].ConnectionString;
        }

        public void Registrar(BitacoraSeguridad registro)
        {
            string query = @"INSERT INTO BitacoraSeguridad
                (Fecha, IdUsuario, NombreUsuario, TipoEvento, Modulo, Accion, Detalle, Gravedad, DireccionIP)
                VALUES
                (@Fecha, @IdUsuario, @NombreUsuario, @TipoEvento, @Modulo, @Accion, @Detalle, @Gravedad, @DireccionIP)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Fecha", registro.Fecha);
                    cmd.Parameters.AddWithValue("@IdUsuario", (object)registro.IdUsuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreUsuario", (object)registro.NombreUsuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TipoEvento", registro.TipoEvento);
                    cmd.Parameters.AddWithValue("@Modulo", registro.Modulo);
                    cmd.Parameters.AddWithValue("@Accion", registro.Accion);
                    cmd.Parameters.AddWithValue("@Detalle", (object)registro.Detalle ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gravedad", registro.Gravedad);
                    cmd.Parameters.AddWithValue("@DireccionIP", (object)registro.DireccionIP ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<BitacoraSeguridad> ObtenerTodos()
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad
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

        public List<BitacoraSeguridad> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad
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

        public List<BitacoraSeguridad> ObtenerPorTipoEvento(string tipoEvento)
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad
                            WHERE TipoEvento = @TipoEvento
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TipoEvento", tipoEvento);

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

        public List<BitacoraSeguridad> ObtenerPorUsuario(Guid idUsuario)
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad
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

        public List<BitacoraSeguridad> ObtenerPorModulo(string modulo)
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad
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

        public List<BitacoraSeguridad> ObtenerPorGravedad(string gravedad)
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad
                            WHERE Gravedad = @Gravedad
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Gravedad", gravedad);

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

        public List<BitacoraSeguridad> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string gravedad = null)
        {
            List<BitacoraSeguridad> lista = new List<BitacoraSeguridad>();
            List<string> condiciones = new List<string>();

            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Gravedad, DireccionIP
                            FROM BitacoraSeguridad WHERE 1=1";

            if (fechaInicio.HasValue)
                condiciones.Add("Fecha >= @FechaInicio");

            if (fechaFin.HasValue)
                condiciones.Add("Fecha <= @FechaFin");

            if (!string.IsNullOrWhiteSpace(tipoEvento))
                condiciones.Add("TipoEvento = @TipoEvento");

            if (idUsuario.HasValue)
                condiciones.Add("IdUsuario = @IdUsuario");

            if (!string.IsNullOrWhiteSpace(modulo))
                condiciones.Add("Modulo = @Modulo");

            if (!string.IsNullOrWhiteSpace(gravedad))
                condiciones.Add("Gravedad = @Gravedad");

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

                    if (!string.IsNullOrWhiteSpace(tipoEvento))
                        cmd.Parameters.AddWithValue("@TipoEvento", tipoEvento);

                    if (idUsuario.HasValue)
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario.Value);

                    if (!string.IsNullOrWhiteSpace(modulo))
                        cmd.Parameters.AddWithValue("@Modulo", modulo);

                    if (!string.IsNullOrWhiteSpace(gravedad))
                        cmd.Parameters.AddWithValue("@Gravedad", gravedad);

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

        private BitacoraSeguridad MapearDesdeDataReader(SqlDataReader reader)
        {
            return new BitacoraSeguridad
            {
                IdBitacora = reader.GetInt32(reader.GetOrdinal("IdBitacora")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                IdUsuario = reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("IdUsuario")),
                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                TipoEvento = reader.GetString(reader.GetOrdinal("TipoEvento")),
                Modulo = reader.GetString(reader.GetOrdinal("Modulo")),
                Accion = reader.GetString(reader.GetOrdinal("Accion")),
                Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString(reader.GetOrdinal("Detalle")),
                Gravedad = reader.GetString(reader.GetOrdinal("Gravedad")),
                DireccionIP = reader.IsDBNull(reader.GetOrdinal("DireccionIP")) ? null : reader.GetString(reader.GetOrdinal("DireccionIP"))
            };
        }
    }
}
