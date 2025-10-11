using System;
using System.Data;
using DomainModel;

namespace DAL.Tools
{
    public class DevolucionAdapter
    {
        public static Devolucion AdaptDevolucion(DataRow row)
        {
            Devolucion devolucion = new Devolucion
            {
                IdDevolucion = (Guid)row["IdDevolucion"],
                IdPrestamo = (Guid)row["IdPrestamo"],
                FechaDevolucion = Convert.ToDateTime(row["FechaDevolucion"]),
                IdUsuario = (Guid)row["IdUsuario"],
                Observaciones = row["Observaciones"] != DBNull.Value ? row["Observaciones"].ToString() : string.Empty
            };

            return devolucion;
        }

        public static Devolucion AdaptDevolucionConPrestamo(DataRow row)
        {
            Devolucion devolucion = AdaptDevolucion(row);

            // Si la consulta incluye join con Prestamo
            if (row.Table.Columns.Contains("FechaDevolucionPrevista"))
            {
                devolucion.Prestamo = new Prestamo
                {
                    IdPrestamo = (Guid)row["IdPrestamo"],
                    FechaPrestamo = Convert.ToDateTime(row["FechaPrestamo"]),
                    FechaDevolucionPrevista = Convert.ToDateTime(row["FechaDevolucionPrevista"]),
                    Estado = row["EstadoPrestamo"]?.ToString()
                };
            }

            return devolucion;
        }
    }
}
