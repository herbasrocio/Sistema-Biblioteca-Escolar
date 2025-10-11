using System;
using System.Data;
using DomainModel;

namespace DAL.Tools
{
    /// <summary>
    /// Adaptador para convertir DataRow a entidad Inscripcion
    /// </summary>
    public class InscripcionAdapter
    {
        /// <summary>
        /// Convierte un DataRow a objeto Inscripcion
        /// </summary>
        public static Inscripcion AdaptInscripcion(DataRow row)
        {
            if (row == null)
                return null;

            Inscripcion inscripcion = new Inscripcion
            {
                IdInscripcion = (Guid)row["IdInscripcion"],
                IdAlumno = (Guid)row["IdAlumno"],
                AnioLectivo = Convert.ToInt32(row["AnioLectivo"]),
                Grado = row["Grado"].ToString(),
                Division = row["Division"] != DBNull.Value ? row["Division"].ToString() : string.Empty,
                FechaInscripcion = Convert.ToDateTime(row["FechaInscripcion"]),
                Estado = row["Estado"].ToString()
            };

            return inscripcion;
        }

        /// <summary>
        /// Convierte un DataRow a objeto Alumno con datos de inscripción
        /// (Usado cuando se hace JOIN entre Alumno e Inscripcion)
        /// </summary>
        public static Alumno AdaptAlumnoConInscripcion(DataRow row)
        {
            if (row == null)
                return null;

            Alumno alumno = new Alumno
            {
                IdAlumno = (Guid)row["IdAlumno"],
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                DNI = row["DNI"].ToString(),
                FechaRegistro = Convert.ToDateTime(row["FechaRegistro"])
            };

            // Obtener Grado y Division de la inscripción
            if (row.Table.Columns.Contains("Grado") && row["Grado"] != DBNull.Value)
                alumno.Grado = row["Grado"].ToString();

            if (row.Table.Columns.Contains("Division") && row["Division"] != DBNull.Value)
                alumno.Division = row["Division"].ToString();

            return alumno;
        }
    }
}
