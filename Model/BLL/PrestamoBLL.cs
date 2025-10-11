using System;
using System.Collections.Generic;
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

        // Constructor con inyección de dependencias
        public PrestamoBLL(IPrestamoRepository prestamoRepository, IMaterialRepository materialRepository, IAlumnoRepository alumnoRepository)
        {
            _prestamoRepository = prestamoRepository ?? throw new ArgumentNullException(nameof(prestamoRepository));
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
            _alumnoRepository = alumnoRepository ?? throw new ArgumentNullException(nameof(alumnoRepository));
        }

        // Constructor sin parámetros
        public PrestamoBLL() : this(new PrestamoRepository(), new MaterialRepository(), new AlumnoRepository()) { }

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

            // Validar que el material existe y está disponible
            var material = _materialRepository.ObtenerPorId(prestamo.IdMaterial);
            if (material == null)
                throw new Exception("El material no existe");

            if (material.CantidadDisponible <= 0)
                throw new Exception($"No hay ejemplares disponibles de '{material.Titulo}'");

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

            // Registrar préstamo
            _prestamoRepository.Add(prestamo);

            // Actualizar cantidad disponible del material
            material.CantidadDisponible--;
            _materialRepository.Update(material);
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

            // Actualizar estado del préstamo
            _prestamoRepository.ActualizarEstado(idPrestamo, "Devuelto");

            // Devolver cantidad disponible del material
            var material = _materialRepository.ObtenerPorId(prestamo.IdMaterial);
            if (material != null)
            {
                material.CantidadDisponible++;
                _materialRepository.Update(material);
            }
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
