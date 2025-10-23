using System;

namespace DomainModel
{
    public class RenovacionPrestamo
    {
        public Guid IdRenovacion { get; set; }
        public Guid IdPrestamo { get; set; }
        public DateTime FechaRenovacion { get; set; }
        public DateTime FechaDevolucionAnterior { get; set; }
        public DateTime FechaDevolucionNueva { get; set; }
        public Guid IdUsuario { get; set; }
        public string Observaciones { get; set; }

        // Propiedades de navegación
        public Prestamo Prestamo { get; set; }

        public RenovacionPrestamo()
        {
            IdRenovacion = Guid.NewGuid();
            FechaRenovacion = DateTime.Now;
        }

        public int DiasExtension
        {
            get
            {
                return (FechaDevolucionNueva - FechaDevolucionAnterior).Days;
            }
        }
    }
}
