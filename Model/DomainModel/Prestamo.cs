using System;

namespace DomainModel
{
    public class Prestamo
    {
        public Guid IdPrestamo { get; set; }
        public Guid IdMaterial { get; set; }
        public Guid IdEjemplar { get; set; }
        public Guid IdAlumno { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionPrevista { get; set; }
        public string Estado { get; set; } // Activo, Devuelto, Atrasado
        public int CantidadRenovaciones { get; set; }
        public DateTime? FechaUltimaRenovacion { get; set; }

        // Propiedades de navegación (opcional para mostrar en UI)
        public Material Material { get; set; }
        public Alumno Alumno { get; set; }
        public Ejemplar Ejemplar { get; set; }

        public Prestamo()
        {
            IdPrestamo = Guid.NewGuid();
            FechaPrestamo = DateTime.Now;
            Estado = "Activo";
            CantidadRenovaciones = 0;
            FechaUltimaRenovacion = null;
        }

        public bool EstaAtrasado()
        {
            return Estado == "Activo" && DateTime.Now > FechaDevolucionPrevista;
        }

        public int DiasRestantes()
        {
            if (Estado != "Activo") return 0;
            return (FechaDevolucionPrevista - DateTime.Now).Days;
        }

        public bool PuedeRenovarse(int maxRenovaciones = 2)
        {
            return Estado == "Activo" && CantidadRenovaciones < maxRenovaciones;
        }
    }
}
