using System;

namespace DomainModel
{
    /// <summary>
    /// Representa un alumno registrado en el sistema de biblioteca escolar.
    /// Contiene información personal y académica del estudiante.
    /// </summary>
    /// <remarks>
    /// Esta clase forma parte del modelo de dominio del sistema de gestión de biblioteca.
    /// Los alumnos pueden realizar préstamos de materiales bibliográficos.
    /// </remarks>
    public class Alumno
    {
        /// <summary>
        /// Obtiene o establece el identificador único del alumno.
        /// </summary>
        /// <value>GUID que identifica de manera única al alumno en el sistema.</value>
        public Guid IdAlumno { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del alumno.
        /// </summary>
        /// <value>Nombre o nombres del alumno.</value>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el apellido del alumno.
        /// </summary>
        /// <value>Apellido o apellidos del alumno.</value>
        public string Apellido { get; set; }

        /// <summary>
        /// Obtiene o establece el Documento Nacional de Identidad del alumno.
        /// </summary>
        /// <value>DNI del alumno sin puntos ni guiones.</value>
        public string DNI { get; set; }

        /// <summary>
        /// Obtiene o establece el grado o año escolar del alumno.
        /// </summary>
        /// <value>Número del grado (ej: "1", "2", "3") o nivel especial (ej: "Jardín").</value>
        public string Grado { get; set; }

        /// <summary>
        /// Obtiene o establece la división o sección del curso.
        /// </summary>
        /// <value>División del curso (ej: "A", "B", "C").</value>
        public string Division { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de registro del alumno en el sistema.
        /// </summary>
        /// <value>Fecha y hora en que se registró el alumno.</value>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Alumno"/>.
        /// </summary>
        /// <remarks>
        /// El constructor genera automáticamente un nuevo identificador único
        /// y establece la fecha de registro al momento actual.
        /// </remarks>
        public Alumno()
        {
            IdAlumno = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
        }

        /// <summary>
        /// Obtiene el nombre completo del alumno en formato "Apellido, Nombre".
        /// </summary>
        /// <value>Cadena con formato "Apellido, Nombre".</value>
        public string NombreCompleto => $"{Apellido}, {Nombre}";

        /// <summary>
        /// Obtiene el grado completo con formato legible incluyendo sufijo ordinal y división.
        /// </summary>
        /// <value>
        /// Cadena formateada con grado ordinal y división (ej: "1ro A", "2do B", "3ro C").
        /// Si el grado no es numérico, devuelve el formato "Grado División".
        /// Si faltan datos, devuelve cadena vacía.
        /// </value>
        /// <example>
        /// Para Grado="1" y Division="A" devuelve "1ro A".
        /// Para Grado="2" y Division="B" devuelve "2do B".
        /// Para Grado="Jardín" y Division="A" devuelve "Jardín A".
        /// </example>
        public string GradoCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(Grado) || string.IsNullOrEmpty(Division))
                    return string.Empty;

                string sufijo = "ro";
                int gradoNum;

                if (int.TryParse(Grado, out gradoNum))
                {
                    switch (gradoNum)
                    {
                        case 1:
                            sufijo = "ro";
                            break;
                        case 2:
                            sufijo = "do";
                            break;
                        case 3:
                            sufijo = "ro";
                            break;
                        default:
                            sufijo = "to";
                            break;
                    }
                    return $"{gradoNum}{sufijo} {Division}";
                }

                // Si no es un número (ej: "Jardín"), devolver tal cual
                return $"{Grado} {Division}";
            }
        }
    }
}
