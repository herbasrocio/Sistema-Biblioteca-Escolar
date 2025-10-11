using System;
using System.Data;
using DomainModel;

namespace DAL.Tools
{
    public class PrestamoAdapter
    {
        public static Prestamo AdaptPrestamo(DataRow row)
        {
            Prestamo prestamo = new Prestamo
            {
                IdPrestamo = (Guid)row["IdPrestamo"],
                IdMaterial = (Guid)row["IdMaterial"],
                IdAlumno = (Guid)row["IdAlumno"],
                IdUsuario = (Guid)row["IdUsuario"],
                FechaPrestamo = Convert.ToDateTime(row["FechaPrestamo"]),
                FechaDevolucionPrevista = Convert.ToDateTime(row["FechaDevolucionPrevista"]),
                Estado = row["Estado"].ToString()
            };

            return prestamo;
        }

        public static Prestamo AdaptPrestamoConRelaciones(DataRow row)
        {
            Prestamo prestamo = AdaptPrestamo(row);

            // Si la consulta incluye joins, adaptar las entidades relacionadas
            if (row.Table.Columns.Contains("TituloMaterial"))
            {
                prestamo.Material = new Material
                {
                    IdMaterial = (Guid)row["IdMaterial"],
                    Titulo = row["TituloMaterial"]?.ToString(),
                    Autor = row["AutorMaterial"]?.ToString()
                };
            }

            if (row.Table.Columns.Contains("NombreAlumno"))
            {
                prestamo.Alumno = new Alumno
                {
                    IdAlumno = (Guid)row["IdAlumno"],
                    Nombre = row["NombreAlumno"]?.ToString(),
                    Apellido = row["ApellidoAlumno"]?.ToString()
                };
            }

            return prestamo;
        }
    }
}
