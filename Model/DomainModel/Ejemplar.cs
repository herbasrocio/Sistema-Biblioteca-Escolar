using System;
using DomainModel.Enums;

namespace DomainModel
{
    /// <summary>
    /// Representa una copia física individual de un material
    /// Cada ejemplar tiene su propio estado independiente
    /// </summary>
    public class Ejemplar
    {
        public Guid IdEjemplar { get; set; }
        public Guid IdMaterial { get; set; }
        public int NumeroEjemplar { get; set; } // Número secuencial (1, 2, 3, etc.)
        public string CodigoBarras { get; set; } // Código de barras único opcional
        public EstadoMaterial Estado { get; set; }
        public string Ubicacion { get; set; } // Ubicación física en la biblioteca (ej: "Estante A3")
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Propiedad de navegación (no se mapea directamente de BD)
        public Material Material { get; set; }

        public Ejemplar()
        {
            IdEjemplar = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            Estado = EstadoMaterial.Disponible;
        }
    }
}
