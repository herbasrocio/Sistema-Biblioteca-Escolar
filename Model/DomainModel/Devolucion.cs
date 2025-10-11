using System;

namespace DomainModel
{
    public class Devolucion
    {
        public Guid IdDevolucion { get; set; }
        public Guid IdPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public Guid IdUsuario { get; set; }
        public string Observaciones { get; set; }

        // Propiedad de navegación
        public Prestamo Prestamo { get; set; }

        public Devolucion()
        {
            IdDevolucion = Guid.NewGuid();
            FechaDevolucion = DateTime.Now;
        }

        public bool FueDevueltoATiempo()
        {
            if (Prestamo == null) return true;
            return FechaDevolucion <= Prestamo.FechaDevolucionPrevista;
        }

        public int DiasDeAtraso()
        {
            if (Prestamo == null || FueDevueltoATiempo()) return 0;
            return (FechaDevolucion - Prestamo.FechaDevolucionPrevista).Days;
        }
    }
}
