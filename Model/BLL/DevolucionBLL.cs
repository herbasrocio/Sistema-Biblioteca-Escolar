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
            // Validaciones previas (no requieren transacción)
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

            // OPERACIONES TRANSACCIONALES: Usar Unit of Work para garantizar atomicidad
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();
                try
                {
                    // 1. Registrar devolución
                    uow.Devoluciones.Add(devolucion);

                    // 2. Actualizar estado del préstamo a "Devuelto"
                    uow.Prestamos.ActualizarEstado(prestamo.IdPrestamo, "Devuelto");

                    // 3. Cambiar estado del ejemplar a Disponible
                    if (prestamo.IdEjemplar != Guid.Empty)
                    {
                        var ejemplar = uow.Ejemplares.ObtenerPorId(prestamo.IdEjemplar);
                        if (ejemplar != null)
                        {
                            ejemplar.Estado = DomainModel.Enums.EstadoMaterial.Disponible;
                            uow.Ejemplares.Update(ejemplar);
                        }
                    }

                    // Confirmar transacción - las 3 operaciones se confirman juntas
                    uow.Commit();

                    // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
                    // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
                }
                catch
                {
                    // Rollback automático al salir del using si no se llamó Commit()
                    uow.Rollback();
                    throw;
                }
            }
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
            // Validar que el préstamo existe
            var prestamo = _prestamoRepository.ObtenerPorId(devolucion.IdPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo asociado no existe");

            // OPERACIONES TRANSACCIONALES: Usar Unit of Work para garantizar atomicidad
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();
                try
                {
                    // 1. Eliminar la devolución
                    uow.Devoluciones.Delete(devolucion);

                    // 2. Cambiar estado del préstamo a Activo o Atrasado según corresponda
                    string nuevoEstado = prestamo.EstaAtrasado() ? "Atrasado" : "Activo";
                    uow.Prestamos.ActualizarEstado(prestamo.IdPrestamo, nuevoEstado);

                    // 3. Cambiar estado del ejemplar a Prestado
                    if (prestamo.IdEjemplar != Guid.Empty)
                    {
                        var ejemplar = uow.Ejemplares.ObtenerPorId(prestamo.IdEjemplar);
                        if (ejemplar != null)
                        {
                            ejemplar.Estado = DomainModel.Enums.EstadoMaterial.Prestado;
                            uow.Ejemplares.Update(ejemplar);
                        }
                    }

                    // Confirmar transacción - las 3 operaciones se confirman juntas
                    uow.Commit();

                    // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
                    // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
                }
                catch
                {
                    // Rollback automático al salir del using si no se llamó Commit()
                    uow.Rollback();
                    throw;
                }
            }
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
