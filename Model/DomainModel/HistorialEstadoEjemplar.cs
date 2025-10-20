using System;
using DomainModel.Enums;

namespace DomainModel
{
    /// <summary>
    /// Representa un registro de cambio de estado en el historial de un ejemplar
    /// Permite auditar todos los cambios de estado para trazabilidad completa
    /// </summary>
    public class HistorialEstadoEjemplar
    {
        public Guid IdHistorial { get; set; }
        public Guid IdEjemplar { get; set; }
        public EstadoMaterial EstadoAnterior { get; set; }
        public EstadoMaterial EstadoNuevo { get; set; }
        public DateTime FechaCambio { get; set; }
        public Guid? IdUsuario { get; set; } // Nullable - puede ser cambio automático del sistema
        public string Motivo { get; set; }
        public Guid? IdPrestamo { get; set; } // Nullable - solo si el cambio fue por préstamo
        public Guid? IdDevolucion { get; set; } // Nullable - solo si el cambio fue por devolución
        public TipoCambioEstado TipoCambio { get; set; }

        // Propiedades de navegación (no se mapean directamente de BD)
        public Ejemplar Ejemplar { get; set; }
        public string NombreUsuario { get; set; } // Para mostrar en UI

        public HistorialEstadoEjemplar()
        {
            IdHistorial = Guid.NewGuid();
            FechaCambio = DateTime.Now;
            TipoCambio = TipoCambioEstado.Manual;
        }

        /// <summary>
        /// Descripción legible del cambio de estado
        /// </summary>
        public string DescripcionCambio
        {
            get
            {
                return $"{TraducirEstado(EstadoAnterior)} → {TraducirEstado(EstadoNuevo)}";
            }
        }

        /// <summary>
        /// Descripción completa del cambio incluyendo tipo y motivo
        /// </summary>
        public string DescripcionCompleta
        {
            get
            {
                string desc = $"{TraducirEstado(EstadoAnterior)} → {TraducirEstado(EstadoNuevo)}";

                if (!string.IsNullOrEmpty(Motivo))
                    desc += $" | {Motivo}";

                desc += $" ({TraducirTipoCambio(TipoCambio)})";

                return desc;
            }
        }

        private string TraducirEstado(EstadoMaterial estado)
        {
            switch (estado)
            {
                case EstadoMaterial.Disponible:
                    return "Disponible";
                case EstadoMaterial.Prestado:
                    return "Prestado";
                case EstadoMaterial.EnReparacion:
                    return "En Reparación";
                case EstadoMaterial.NoDisponible:
                    return "No Disponible";
                default:
                    return estado.ToString();
            }
        }

        private string TraducirTipoCambio(TipoCambioEstado tipo)
        {
            switch (tipo)
            {
                case TipoCambioEstado.Manual:
                    return "Cambio Manual";
                case TipoCambioEstado.Prestamo:
                    return "Por Préstamo";
                case TipoCambioEstado.Devolucion:
                    return "Por Devolución";
                case TipoCambioEstado.Sistema:
                    return "Automático del Sistema";
                default:
                    return tipo.ToString();
            }
        }
    }

    /// <summary>
    /// Tipos de cambio de estado posibles
    /// </summary>
    public enum TipoCambioEstado
    {
        Manual = 0,      // Cambio manual desde Gestionar Ejemplares
        Prestamo = 1,    // Cambio automático al registrar préstamo
        Devolucion = 2,  // Cambio automático al registrar devolución
        Sistema = 3      // Cambio automático del sistema (ej: mantenimiento)
    }
}
