using System;

namespace DomainModel
{
    public class Alumno
    {
        public Guid IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Grado { get; set; }
        public string Division { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Alumno()
        {
            IdAlumno = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
        }

        public string NombreCompleto => $"{Apellido}, {Nombre}";

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
