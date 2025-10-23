using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly string _connectionString;

        public PrestamoRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NegocioConString"].ConnectionString;
        }

        public void Add(Prestamo entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Prestamo (IdPrestamo, IdMaterial, IdEjemplar, IdAlumno, IdUsuario, FechaPrestamo, FechaDevolucionPrevista, Estado, CantidadRenovaciones, FechaUltimaRenovacion)
                    VALUES (@IdPrestamo, @IdMaterial, @IdEjemplar, @IdAlumno, @IdUsuario, @FechaPrestamo, @FechaDevolucionPrevista, @Estado, @CantidadRenovaciones, @FechaUltimaRenovacion)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", entity.IdPrestamo);
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@IdEjemplar", entity.IdEjemplar);
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@FechaPrestamo", entity.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@FechaDevolucionPrevista", entity.FechaDevolucionPrevista);
                    cmd.Parameters.AddWithValue("@Estado", entity.Estado);
                    cmd.Parameters.AddWithValue("@CantidadRenovaciones", entity.CantidadRenovaciones);
                    cmd.Parameters.AddWithValue("@FechaUltimaRenovacion", (object)entity.FechaUltimaRenovacion ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Prestamo entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Prestamo
                    SET IdMaterial = @IdMaterial,
                        IdEjemplar = @IdEjemplar,
                        IdAlumno = @IdAlumno,
                        IdUsuario = @IdUsuario,
                        FechaPrestamo = @FechaPrestamo,
                        FechaDevolucionPrevista = @FechaDevolucionPrevista,
                        Estado = @Estado,
                        CantidadRenovaciones = @CantidadRenovaciones,
                        FechaUltimaRenovacion = @FechaUltimaRenovacion
                    WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", entity.IdPrestamo);
                    cmd.Parameters.AddWithValue("@IdMaterial", entity.IdMaterial);
                    cmd.Parameters.AddWithValue("@IdEjemplar", entity.IdEjemplar);
                    cmd.Parameters.AddWithValue("@IdAlumno", entity.IdAlumno);
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@FechaPrestamo", entity.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@FechaDevolucionPrevista", entity.FechaDevolucionPrevista);
                    cmd.Parameters.AddWithValue("@Estado", entity.Estado);
                    cmd.Parameters.AddWithValue("@CantidadRenovaciones", entity.CantidadRenovaciones);
                    cmd.Parameters.AddWithValue("@FechaUltimaRenovacion", (object)entity.FechaUltimaRenovacion ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Prestamo entity)
        {
            // No se borran físicamente, se cambia el estado
            entity.Estado = "Cancelado";
            Update(entity);
        }

        public List<Prestamo> GetAll()
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Prestamo ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public Prestamo ObtenerPorId(Guid idPrestamo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Prestamo WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return PrestamoAdapter.AdaptPrestamo(dt.Rows[0]);
                    }
                }
            }

            return null;
        }

        public List<Prestamo> ObtenerPorAlumno(Guid idAlumno)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE IdAlumno = @IdAlumno
                    ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public List<Prestamo> ObtenerPorMaterial(Guid idMaterial)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE IdMaterial = @IdMaterial
                    ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdMaterial", idMaterial);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public List<Prestamo> ObtenerActivos()
        {
            return ObtenerPorEstado("Activo");
        }

        public List<Prestamo> ObtenerAtrasados()
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE Estado = 'Activo'
                    AND FechaDevolucionPrevista < GETDATE()
                    ORDER BY FechaDevolucionPrevista";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public List<Prestamo> ObtenerPorEstado(string estado)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM Prestamo
                    WHERE Estado = @Estado
                    ORDER BY FechaPrestamo DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        prestamos.Add(PrestamoAdapter.AdaptPrestamo(row));
                    }
                }
            }

            return prestamos;
        }

        public void ActualizarEstado(Guid idPrestamo, string nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Prestamo
                    SET Estado = @Estado
                    WHERE IdPrestamo = @IdPrestamo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                    cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable BuscarPrestamosActivos(string nombreAlumno = null, string tituloMaterial = null, string codigoEjemplar = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT
                        p.IdPrestamo,
                        p.IdMaterial,
                        p.IdEjemplar,
                        p.IdAlumno,
                        p.IdUsuario,
                        p.FechaPrestamo,
                        p.FechaDevolucionPrevista,
                        p.Estado,
                        p.CantidadRenovaciones,
                        p.FechaUltimaRenovacion,
                        a.Nombre + ' ' + a.Apellido AS NombreAlumno,
                        a.DNI AS DNIAlumno,
                        m.Titulo AS TituloMaterial,
                        m.Autor,
                        e.CodigoEjemplar,
                        e.NumeroEjemplar,
                        e.Ubicacion,
                        DATEDIFF(day, GETDATE(), p.FechaDevolucionPrevista) AS DiasRestantes,
                        CASE
                            WHEN p.FechaDevolucionPrevista < GETDATE() THEN 1
                            ELSE 0
                        END AS EstaVencido,
                        CASE
                            WHEN p.FechaDevolucionPrevista < GETDATE() THEN ABS(DATEDIFF(day, p.FechaDevolucionPrevista, GETDATE()))
                            ELSE 0
                        END AS DiasAtraso
                    FROM Prestamo p
                    INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
                    INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
                    INNER JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
                    WHERE (p.Estado = 'Activo' OR p.Estado = 'Atrasado')
                        AND (@NombreAlumno IS NULL OR (a.Nombre + ' ' + a.Apellido) LIKE '%' + @NombreAlumno + '%')
                        AND (@TituloMaterial IS NULL OR m.Titulo LIKE '%' + @TituloMaterial + '%')
                        AND (@CodigoEjemplar IS NULL OR e.CodigoEjemplar LIKE '%' + @CodigoEjemplar + '%')
                    ORDER BY
                        CASE WHEN p.FechaDevolucionPrevista < GETDATE() THEN 0 ELSE 1 END,
                        p.FechaDevolucionPrevista ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreAlumno", string.IsNullOrWhiteSpace(nombreAlumno) ? (object)DBNull.Value : nombreAlumno);
                    cmd.Parameters.AddWithValue("@TituloMaterial", string.IsNullOrWhiteSpace(tituloMaterial) ? (object)DBNull.Value : tituloMaterial);
                    cmd.Parameters.AddWithValue("@CodigoEjemplar", string.IsNullOrWhiteSpace(codigoEjemplar) ? (object)DBNull.Value : codigoEjemplar);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
        }

        public void RenovarPrestamo(Guid idPrestamo, DateTime nuevaFechaDevolucion, Guid idUsuario, string observaciones = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Obtener fecha de devolución actual
                    string queryGetFecha = "SELECT FechaDevolucionPrevista, CantidadRenovaciones FROM Prestamo WHERE IdPrestamo = @IdPrestamo";
                    DateTime fechaAnterior;
                    int cantidadActual;

                    using (SqlCommand cmd = new SqlCommand(queryGetFecha, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new Exception("Préstamo no encontrado");

                            fechaAnterior = reader.GetDateTime(0);
                            cantidadActual = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        }
                    }

                    // 2. Actualizar el préstamo
                    string queryUpdate = @"
                        UPDATE Prestamo
                        SET FechaDevolucionPrevista = @NuevaFecha,
                            CantidadRenovaciones = @CantidadRenovaciones,
                            FechaUltimaRenovacion = @FechaRenovacion
                        WHERE IdPrestamo = @IdPrestamo";

                    using (SqlCommand cmd = new SqlCommand(queryUpdate, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                        cmd.Parameters.AddWithValue("@NuevaFecha", nuevaFechaDevolucion);
                        cmd.Parameters.AddWithValue("@CantidadRenovaciones", cantidadActual + 1);
                        cmd.Parameters.AddWithValue("@FechaRenovacion", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Registrar en tabla de auditoría
                    string queryInsertRenovacion = @"
                        INSERT INTO RenovacionPrestamo (IdRenovacion, IdPrestamo, FechaRenovacion, FechaDevolucionAnterior, FechaDevolucionNueva, IdUsuario, Observaciones)
                        VALUES (@IdRenovacion, @IdPrestamo, @FechaRenovacion, @FechaAnterior, @FechaNueva, @IdUsuario, @Observaciones)";

                    using (SqlCommand cmd = new SqlCommand(queryInsertRenovacion, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@IdRenovacion", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                        cmd.Parameters.AddWithValue("@FechaRenovacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@FechaAnterior", fechaAnterior);
                        cmd.Parameters.AddWithValue("@FechaNueva", nuevaFechaDevolucion);
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@Observaciones", (object)observaciones ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<RenovacionPrestamo> ObtenerRenovacionesPorPrestamo(Guid idPrestamo)
        {
            List<RenovacionPrestamo> renovaciones = new List<RenovacionPrestamo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT * FROM RenovacionPrestamo
                    WHERE IdPrestamo = @IdPrestamo
                    ORDER BY FechaRenovacion DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        renovaciones.Add(RenovacionPrestamoAdapter.AdaptRenovacion(row));
                    }
                }
            }

            return renovaciones;
        }
    }
}
