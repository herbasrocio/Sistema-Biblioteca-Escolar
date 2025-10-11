using System;
using System.Data;
using DomainModel;

namespace DAL.Tools
{
    public class AlumnoAdapter
    {
        public static Alumno AdaptAlumno(DataRow row)
        {
            Alumno alumno = new Alumno
            {
                IdAlumno = (Guid)row["IdAlumno"],
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                DNI = row["DNI"].ToString(),
                Grado = row["Grado"] != DBNull.Value ? row["Grado"].ToString() : string.Empty,
                Division = row["Division"] != DBNull.Value ? row["Division"].ToString() : string.Empty,
                FechaRegistro = Convert.ToDateTime(row["FechaRegistro"])
            };

            return alumno;
        }
    }
}
