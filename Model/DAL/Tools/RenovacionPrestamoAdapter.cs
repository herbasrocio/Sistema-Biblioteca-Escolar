using System;
using System.Data;
using DomainModel;

namespace DAL.Tools
{
    public class RenovacionPrestamoAdapter
    {
        public static RenovacionPrestamo AdaptRenovacion(DataRow row)
        {
            RenovacionPrestamo renovacion = new RenovacionPrestamo
            {
                IdRenovacion = (Guid)row["IdRenovacion"],
                IdPrestamo = (Guid)row["IdPrestamo"],
                FechaRenovacion = Convert.ToDateTime(row["FechaRenovacion"]),
                FechaDevolucionAnterior = Convert.ToDateTime(row["FechaDevolucionAnterior"]),
                FechaDevolucionNueva = Convert.ToDateTime(row["FechaDevolucionNueva"]),
                IdUsuario = (Guid)row["IdUsuario"]
            };

            if (row.Table.Columns.Contains("Observaciones") && row["Observaciones"] != DBNull.Value)
            {
                renovacion.Observaciones = row["Observaciones"].ToString();
            }

            return renovacion;
        }
    }
}
