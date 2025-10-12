using System;
using System.Data;
using DomainModel;
using DomainModel.Enums;

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
                Tipo = ParseTipoMaterial(row["Tipo"].ToString()),
                Genero = row["Genero"] != DBNull.Value ? row["Genero"].ToString() : string.Empty,
                ISBN = row.Table.Columns.Contains("ISBN") && row["ISBN"] != DBNull.Value ? row["ISBN"].ToString() : string.Empty,
                AnioPublicacion = row.Table.Columns.Contains("AnioPublicacion") && row["AnioPublicacion"] != DBNull.Value ? (int?)Convert.ToInt32(row["AnioPublicacion"]) : null,
                EdadRecomendada = row.Table.Columns.Contains("EdadRecomendada") && row["EdadRecomendada"] != DBNull.Value ? row["EdadRecomendada"].ToString() : string.Empty,
                Descripcion = row.Table.Columns.Contains("Descripcion") && row["Descripcion"] != DBNull.Value ? row["Descripcion"].ToString() : string.Empty,
                CantidadTotal = Convert.ToInt32(row["CantidadTotal"]),
                CantidadDisponible = Convert.ToInt32(row["CantidadDisponible"]),
                FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };

            return material;
        }

        private static TipoMaterial ParseTipoMaterial(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                return TipoMaterial.Libro; // Valor por defecto

            // Intentar parsear el string al enum
            if (Enum.TryParse<TipoMaterial>(tipo, true, out TipoMaterial resultado))
                return resultado;

            // Si no se puede parsear, retornar el valor por defecto
            return TipoMaterial.Libro;
        }
    }
}
