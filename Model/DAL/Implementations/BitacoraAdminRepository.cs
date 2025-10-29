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
    public class BitacoraAdminRepository : IBitacoraAdminRepository
    {
        private readonly string _connectionString;

        public BitacoraAdminRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ServicesConString"].ConnectionString;
        }

        public void Registrar(BitacoraAdmin registro)
        {
            string query = @"INSERT INTO BitacoraAdmin
                (Fecha, IdUsuario, NombreUsuario, TipoEvento, Modulo, Accion, Detalle, Criticidad, DireccionIP)
                VALUES
                (@Fecha, @IdUsuario, @NombreUsuario, @TipoEvento, @Modulo, @Accion, @Detalle, @Criticidad, @DireccionIP)";

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
                    cmd.Parameters.AddWithValue("@Criticidad", registro.Criticidad);
                    cmd.Parameters.AddWithValue("@DireccionIP", (object)registro.DireccionIP ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<BitacoraAdmin> ObtenerTodos()
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin
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

        public List<BitacoraAdmin> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin
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

        public List<BitacoraAdmin> ObtenerPorTipoEvento(string tipoEvento)
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin
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

        public List<BitacoraAdmin> ObtenerPorUsuario(Guid idUsuario)
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin
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

        public List<BitacoraAdmin> ObtenerPorModulo(string modulo)
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin
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

        public List<BitacoraAdmin> ObtenerPorCriticidad(string criticidad)
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin
                            WHERE Criticidad = @Criticidad
                            ORDER BY Fecha DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Criticidad", criticidad);

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

        public List<BitacoraAdmin> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string criticidad = null)
        {
            List<BitacoraAdmin> lista = new List<BitacoraAdmin>();
            List<string> condiciones = new List<string>();

            string query = @"SELECT IdBitacora, Fecha, IdUsuario, NombreUsuario, TipoEvento,
                            Modulo, Accion, Detalle, Criticidad, DireccionIP
                            FROM BitacoraAdmin WHERE 1=1";

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

            if (!string.IsNullOrWhiteSpace(criticidad))
                condiciones.Add("Criticidad = @Criticidad");

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

                    if (!string.IsNullOrWhiteSpace(criticidad))
                        cmd.Parameters.AddWithValue("@Criticidad", criticidad);

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

        private BitacoraAdmin MapearDesdeDataReader(SqlDataReader reader)
        {
            return new BitacoraAdmin
            {
                IdBitacora = reader.GetInt32(reader.GetOrdinal("IdBitacora")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                IdUsuario = reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("IdUsuario")),
                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                TipoEvento = reader.GetString(reader.GetOrdinal("TipoEvento")),
                Modulo = reader.GetString(reader.GetOrdinal("Modulo")),
                Accion = reader.GetString(reader.GetOrdinal("Accion")),
                Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString(reader.GetOrdinal("Detalle")),
                Criticidad = reader.GetString(reader.GetOrdinal("Criticidad")),
                DireccionIP = reader.IsDBNull(reader.GetOrdinal("DireccionIP")) ? null : reader.GetString(reader.GetOrdinal("DireccionIP"))
            };
        }
    }
}
