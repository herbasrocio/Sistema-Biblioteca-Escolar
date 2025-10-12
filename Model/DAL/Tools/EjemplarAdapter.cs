using System;
using System.Data;
using DomainModel;
using DomainModel.Enums;

namespace DAL.Tools
{
    public class EjemplarAdapter
    {
        public static Ejemplar AdaptEjemplar(DataRow row)
        {
            Ejemplar ejemplar = new Ejemplar
            {
                IdEjemplar = (Guid)row["IdEjemplar"],
                IdMaterial = (Guid)row["IdMaterial"],
                NumeroEjemplar = Convert.ToInt32(row["NumeroEjemplar"]),
                CodigoBarras = row["CodigoBarras"] != DBNull.Value ? row["CodigoBarras"].ToString() : string.Empty,
                Estado = (EstadoMaterial)Convert.ToInt32(row["Estado"]),
                Ubicacion = row["Ubicacion"] != DBNull.Value ? row["Ubicacion"].ToString() : string.Empty,
                Observaciones = row["Observaciones"] != DBNull.Value ? row["Observaciones"].ToString() : string.Empty,
                FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };

            return ejemplar;
        }
    }
}
