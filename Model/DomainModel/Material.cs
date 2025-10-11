using System;

namespace DomainModel
{
    public class Material
    {
        public Guid IdMaterial { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public string Tipo { get; set; } // Libro, Revista, Manual
        public string Genero { get; set; } // Fantasia, Terror, Comedia, etc.
        public int CantidadTotal { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        public Material()
        {
            IdMaterial = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
        }
    }
}
