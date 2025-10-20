using System;
using System.Collections.Generic;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    public class DevolucionBLL
    {
        private readonly IDevolucionRepository _devolucionRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IMaterialRepository _materialRepository;

        // Constructor con inyección de dependencias
        public DevolucionBLL(IDevolucionRepository devolucionRepository, IPrestamoRepository prestamoRepository, IMaterialRepository materialRepository)
        {
            _devolucionRepository = devolucionRepository ?? throw new ArgumentNullException(nameof(devolucionRepository));
            _prestamoRepository = prestamoRepository ?? throw new ArgumentNullException(nameof(prestamoRepository));
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        }

        // Constructor sin parámetros
        public DevolucionBLL() : this(new DevolucionRepository(), new PrestamoRepository(), new MaterialRepository()) { }

        public List<Devolucion> ObtenerTodasDevoluciones()
        {
            return _devolucionRepository.GetAll();
        }

        public Devolucion ObtenerDevolucionPorId(Guid idDevolucion)
        {
            return _devolucionRepository.ObtenerPorId(idDevolucion);
        }

        public Devolucion ObtenerPorPrestamo(Guid idPrestamo)
        {
            return _devolucionRepository.ObtenerPorPrestamo(idPrestamo);
        }

        public List<Devolucion> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return _devolucionRepository.ObtenerPorFecha(fechaInicio, fechaFin);
        }

        public List<Devolucion> ObtenerDevolucionesAtrasadas()
        {
            return _devolucionRepository.ObtenerDevolucionesAtrasadas();
        }

        public void RegistrarDevolucion(Devolucion devolucion)
        {
            // Validaciones
            if (devolucion.IdPrestamo == Guid.Empty)
                throw new Exception("Debe seleccionar un préstamo");

            if (devolucion.IdUsuario == Guid.Empty)
                throw new Exception("Usuario no válido");

            // Validar que el préstamo existe
            var prestamo = _prestamoRepository.ObtenerPorId(devolucion.IdPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo no existe");

            if (prestamo.Estado == "Devuelto")
                throw new Exception("Este préstamo ya fue devuelto anteriormente");

            // Validar que no exista ya una devolución para este préstamo
            var devolucionExistente = _devolucionRepository.ObtenerPorPrestamo(devolucion.IdPrestamo);
            if (devolucionExistente != null)
                throw new Exception("Ya existe una devolución registrada para este préstamo");

            // Registrar devolución
            _devolucionRepository.Add(devolucion);

            // Actualizar estado del préstamo a "Devuelto"
            _prestamoRepository.ActualizarEstado(prestamo.IdPrestamo, "Devuelto");

            // Cambiar estado del ejemplar a Disponible
            if (prestamo.IdEjemplar != Guid.Empty)
            {
                var ejemplarRepository = new EjemplarRepository();
                var ejemplar = ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);
                if (ejemplar != null)
                {
                    ejemplar.Estado = DomainModel.Enums.EstadoMaterial.Disponible;
                    ejemplarRepository.Update(ejemplar);
                }
            }

            // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
            // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
        }

        public void ActualizarDevolucion(Devolucion devolucion)
        {
            // Validaciones
            if (devolucion.IdPrestamo == Guid.Empty)
                throw new Exception("Debe seleccionar un préstamo");

            if (devolucion.IdUsuario == Guid.Empty)
                throw new Exception("Usuario no válido");

            _devolucionRepository.Update(devolucion);
        }

        public void EliminarDevolucion(Devolucion devolucion)
        {
            // Al eliminar una devolución, reactivar el préstamo y ajustar inventario
            var prestamo = _prestamoRepository.ObtenerPorId(devolucion.IdPrestamo);
            if (prestamo != null)
            {
                // Cambiar estado del préstamo a Activo o Atrasado según corresponda
                string nuevoEstado = prestamo.EstaAtrasado() ? "Atrasado" : "Activo";
                _prestamoRepository.ActualizarEstado(prestamo.IdPrestamo, nuevoEstado);

                // Cambiar estado del ejemplar a Prestado
                if (prestamo.IdEjemplar != Guid.Empty)
                {
                    var ejemplarRepository = new EjemplarRepository();
                    var ejemplar = ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);
                    if (ejemplar != null)
                    {
                        ejemplar.Estado = DomainModel.Enums.EstadoMaterial.Prestado;
                        ejemplarRepository.Update(ejemplar);
                    }
                }

                // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
                // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
            }

            _devolucionRepository.Delete(devolucion);
        }

        public int CalcularDiasAtraso(Guid idDevolucion)
        {
            var devolucion = _devolucionRepository.ObtenerPorId(idDevolucion);
            if (devolucion == null) return 0;

            var prestamo = _prestamoRepository.ObtenerPorId(devolucion.IdPrestamo);
            if (prestamo == null) return 0;

            devolucion.Prestamo = prestamo;
            return devolucion.DiasDeAtraso();
        }
    }
}
