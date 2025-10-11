using System;
using System.Data;
using DomainModel;

namespace DAL.Tools
{
    public class MaterialAdapter
    {
        public static Material AdaptMaterial(DataRow row)
        {
            Material material = new Material
            {
                IdMaterial = (Guid)row["IdMaterial"],
                Titulo = row["Titulo"].ToString(),
                Autor = row["Autor"].ToString(),
                Editorial = row["Editorial"] != DBNull.Value ? row["Editorial"].ToString() : string.Empty,
                Tipo = row["Tipo"].ToString(),
                Genero = row["Genero"].ToString(),
                CantidadTotal = Convert.ToInt32(row["CantidadTotal"]),
                CantidadDisponible = Convert.ToInt32(row["CantidadDisponible"]),
                FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };

            return material;
        }
    }
}
