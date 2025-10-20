using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    public class PrestamoBLL
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IEjemplarRepository _ejemplarRepository;

        // Constructor con inyección de dependencias
        public PrestamoBLL(IPrestamoRepository prestamoRepository, IMaterialRepository materialRepository, IAlumnoRepository alumnoRepository, IEjemplarRepository ejemplarRepository)
        {
            _prestamoRepository = prestamoRepository ?? throw new ArgumentNullException(nameof(prestamoRepository));
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
            _alumnoRepository = alumnoRepository ?? throw new ArgumentNullException(nameof(alumnoRepository));
            _ejemplarRepository = ejemplarRepository ?? throw new ArgumentNullException(nameof(ejemplarRepository));
        }

        // Constructor sin parámetros
        public PrestamoBLL() : this(new PrestamoRepository(), new MaterialRepository(), new AlumnoRepository(), new EjemplarRepository()) { }

        public List<Prestamo> ObtenerTodosPrestamos()
        {
            return _prestamoRepository.GetAll();
        }

        public Prestamo ObtenerPrestamoPorId(Guid idPrestamo)
        {
            return _prestamoRepository.ObtenerPorId(idPrestamo);
        }

        public List<Prestamo> ObtenerPorAlumno(Guid idAlumno)
        {
            return _prestamoRepository.ObtenerPorAlumno(idAlumno);
        }

        public List<Prestamo> ObtenerPorMaterial(Guid idMaterial)
        {
            return _prestamoRepository.ObtenerPorMaterial(idMaterial);
        }

        public List<Prestamo> ObtenerPrestamosActivos()
        {
            return _prestamoRepository.ObtenerActivos();
        }

        public List<Prestamo> ObtenerPrestamosAtrasados()
        {
            return _prestamoRepository.ObtenerAtrasados();
        }

        public void RegistrarPrestamo(Prestamo prestamo)
        {
            // Validaciones
            if (prestamo.IdMaterial == Guid.Empty)
                throw new Exception("Debe seleccionar un material");

            if (prestamo.IdAlumno == Guid.Empty)
                throw new Exception("Debe seleccionar un alumno");

            if (prestamo.IdUsuario == Guid.Empty)
                throw new Exception("Usuario no válido");

            if (prestamo.FechaDevolucionPrevista <= prestamo.FechaPrestamo)
                throw new Exception("La fecha de devolución prevista debe ser posterior a la fecha de préstamo");

            // Validar que el material existe
            var material = _materialRepository.ObtenerPorId(prestamo.IdMaterial);
            if (material == null)
                throw new Exception("El material no existe");

            // Validar que el alumno existe
            var alumno = _alumnoRepository.ObtenerPorId(prestamo.IdAlumno);
            if (alumno == null)
                throw new Exception("El alumno no existe");

            // Verificar que el alumno no tenga préstamos atrasados
            var prestamosAlumno = _prestamoRepository.ObtenerPorAlumno(prestamo.IdAlumno);
            foreach (var p in prestamosAlumno)
            {
                if (p.EstaAtrasado())
                    throw new Exception($"El alumno {alumno.NombreCompleto} tiene préstamos atrasados. Debe devolverlos antes de realizar un nuevo préstamo.");
            }

            // Validar que se haya seleccionado un ejemplar específico
            if (prestamo.IdEjemplar == Guid.Empty)
                throw new Exception("Debe seleccionar un ejemplar específico del material");

            // Obtener el ejemplar seleccionado y validar que está disponible
            var ejemplarSeleccionado = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);

            if (ejemplarSeleccionado == null)
                throw new Exception("El ejemplar seleccionado no existe");

            if (ejemplarSeleccionado.IdMaterial != prestamo.IdMaterial)
                throw new Exception("El ejemplar seleccionado no corresponde al material elegido");

            if (ejemplarSeleccionado.Estado != DomainModel.Enums.EstadoMaterial.Disponible)
                throw new Exception($"El ejemplar seleccionado no está disponible. Estado actual: {ejemplarSeleccionado.Estado}");

            // Cambiar estado del ejemplar a Prestado
            ejemplarSeleccionado.Estado = DomainModel.Enums.EstadoMaterial.Prestado;
            _ejemplarRepository.Update(ejemplarSeleccionado);

            // Generar GUID para el préstamo si no existe
            if (prestamo.IdPrestamo == Guid.Empty)
                prestamo.IdPrestamo = Guid.NewGuid();

            // Registrar préstamo
            _prestamoRepository.Add(prestamo);

            // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
            // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
        }

        public void ActualizarEstadoPrestamo(Guid idPrestamo, string nuevoEstado)
        {
            var prestamo = _prestamoRepository.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo no existe");

            _prestamoRepository.ActualizarEstado(idPrestamo, nuevoEstado);
        }

        public void MarcarComoDevuelto(Guid idPrestamo)
        {
            var prestamo = _prestamoRepository.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo no existe");

            if (prestamo.Estado == "Devuelto")
                throw new Exception("Este préstamo ya fue devuelto");

            // Cambiar estado del ejemplar a Disponible
            if (prestamo.IdEjemplar != Guid.Empty)
            {
                var ejemplar = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);
                if (ejemplar != null)
                {
                    ejemplar.Estado = DomainModel.Enums.EstadoMaterial.Disponible;
                    _ejemplarRepository.Update(ejemplar);
                }
            }

            // Actualizar estado del préstamo
            _prestamoRepository.ActualizarEstado(idPrestamo, "Devuelto");

            // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
            // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
        }

        public void ActualizarPrestamosAtrasados()
        {
            // Actualizar automáticamente el estado de préstamos atrasados
            var prestamosActivos = _prestamoRepository.ObtenerActivos();
            foreach (var prestamo in prestamosActivos)
            {
                if (prestamo.EstaAtrasado())
                {
                    _prestamoRepository.ActualizarEstado(prestamo.IdPrestamo, "Atrasado");
                }
            }
        }
    }
}
