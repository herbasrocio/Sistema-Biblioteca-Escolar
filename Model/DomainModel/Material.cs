using System;
using DomainModel.Enums;

namespace DomainModel
{
    /// <summary>
    /// Representa un material del catálogo de la biblioteca (concepto general)
    /// Ejemplo: "El Principito" como libro genérico
    /// Los estados individuales ahora se manejan en cada Ejemplar (copia física)
    /// </summary>
    public class Material
    {
        public Guid IdMaterial { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public TipoMaterial Tipo { get; set; } // Libro, Revista, Manual (usando enum)
        public string Genero { get; set; } // Fantasia, Terror, Comedia, etc.
        public string ISBN { get; set; }
        public int? AnioPublicacion { get; set; }
        public string Nivel { get; set; } // Inicial, Primario, Secundario, Universitario
        public int CantidadTotal { get; set; } // Total de ejemplares (calculado desde Ejemplar)
        public int CantidadDisponible { get; set; } // Ejemplares disponibles (calculado desde Ejemplar)
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        public Material()
        {
            IdMaterial = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            CantidadDisponible = 0;
            CantidadTotal = 0;
        }
    }
}
