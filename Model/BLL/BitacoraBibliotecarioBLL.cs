using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using System;
using System.Collections.Generic;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para BitacoraBibliotecario.
    /// Proporciona métodos para registrar y consultar operaciones de bibliotecarios.
    /// </summary>
    public class BitacoraBibliotecarioBLL
    {
        private readonly IBitacoraBibliotecarioRepository _repository;

        public BitacoraBibliotecarioBLL(IBitacoraBibliotecarioRepository repository)
        {
            _repository = repository;
        }

        public BitacoraBibliotecarioBLL() : this(new BitacoraBibliotecarioRepository()) { }

        /// <summary>
        /// Registra una operación de préstamo en la bitácora
        /// </summary>
        public void RegistrarPrestamo(int idPrestamo, Guid idUsuario, string nombreUsuario, string detalle = null)
        {
            var registro = new BitacoraBibliotecario
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoOperacion = "Prestamo",
                Modulo = "Transacciones - Préstamos",
                Accion = "Registrar préstamo",
                EntidadAfectada = "Prestamo",
                IdEntidad = idPrestamo,
                Detalle = detalle
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra una operación de devolución en la bitácora
        /// </summary>
        public void RegistrarDevolucion(int idDevolucion, Guid idUsuario, string nombreUsuario, string detalle = null)
        {
            var registro = new BitacoraBibliotecario
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoOperacion = "Devolucion",
                Modulo = "Transacciones - Devoluciones",
                Accion = "Registrar devolución",
                EntidadAfectada = "Devolucion",
                IdEntidad = idDevolucion,
                Detalle = detalle
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra una operación de renovación de préstamo en la bitácora
        /// </summary>
        public void RegistrarRenovacion(int idPrestamo, Guid idUsuario, string nombreUsuario, string detalle = null)
        {
            var registro = new BitacoraBibliotecario
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoOperacion = "Renovacion",
                Modulo = "Transacciones - Renovaciones",
                Accion = "Renovar préstamo",
                EntidadAfectada = "Prestamo",
                IdEntidad = idPrestamo,
                Detalle = detalle
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra una operación de gestión de materiales
        /// </summary>
        public void RegistrarOperacionMaterial(string accion, int? idMaterial, Guid? idUsuario,
            string nombreUsuario, string detalle = null)
        {
            var registro = new BitacoraBibliotecario
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoOperacion = "GestionMaterial",
                Modulo = "Administración - Materiales",
                Accion = accion,
                EntidadAfectada = "Material",
                IdEntidad = idMaterial,
                Detalle = detalle
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra una operación de gestión de alumnos
        /// </summary>
        public void RegistrarOperacionAlumno(string accion, int? idAlumno, Guid? idUsuario,
            string nombreUsuario, string detalle = null)
        {
            var registro = new BitacoraBibliotecario
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoOperacion = "GestionAlumno",
                Modulo = "Administración - Alumnos",
                Accion = accion,
                EntidadAfectada = "Alumno",
                IdEntidad = idAlumno,
                Detalle = detalle
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra una consulta de material
        /// </summary>
        public void RegistrarConsultaMaterial(Guid? idUsuario, string nombreUsuario, string detalle = null)
        {
            var registro = new BitacoraBibliotecario
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoOperacion = "ConsultaMaterial",
                Modulo = "Administración - Consultar Material",
                Accion = "Consultar catálogo de materiales",
                EntidadAfectada = "Material",
                Detalle = detalle
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra una operación genérica en la bitácora de bibliotecario
        /// </summary>
        public void RegistrarOperacion(BitacoraBibliotecario registro)
        {
            if (registro == null)
                throw new ArgumentNullException(nameof(registro), "El registro de bitácora no puede ser nulo");

            if (string.IsNullOrWhiteSpace(registro.TipoOperacion))
                throw new ArgumentException("El tipo de operación es obligatorio");

            if (string.IsNullOrWhiteSpace(registro.Modulo))
                throw new ArgumentException("El módulo es obligatorio");

            if (string.IsNullOrWhiteSpace(registro.Accion))
                throw new ArgumentException("La acción es obligatoria");

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Obtiene todos los registros de la bitácora
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");

            return _repository.ObtenerPorFechas(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Obtiene registros filtrados por tipo de operación
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerPorTipoOperacion(string tipoOperacion)
        {
            if (string.IsNullOrWhiteSpace(tipoOperacion))
                throw new ArgumentException("El tipo de operación no puede estar vacío");

            return _repository.ObtenerPorTipoOperacion(tipoOperacion);
        }

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerPorUsuario(Guid idUsuario)
        {
            if (idUsuario == Guid.Empty)
                throw new ArgumentException("El ID de usuario no puede estar vacío");

            return _repository.ObtenerPorUsuario(idUsuario);
        }

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerPorModulo(string modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo))
                throw new ArgumentException("El módulo no puede estar vacío");

            return _repository.ObtenerPorModulo(modulo);
        }

        /// <summary>
        /// Obtiene registros filtrados por entidad afectada
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerPorEntidad(string entidadAfectada, int? idEntidad = null)
        {
            if (string.IsNullOrWhiteSpace(entidadAfectada))
                throw new ArgumentException("La entidad afectada no puede estar vacía");

            return _repository.ObtenerPorEntidad(entidadAfectada, idEntidad);
        }

        /// <summary>
        /// Obtiene registros con múltiples filtros opcionales
        /// </summary>
        public List<BitacoraBibliotecario> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoOperacion = null, Guid? idUsuario = null, string modulo = null, string entidadAfectada = null)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");

            return _repository.ObtenerConFiltros(fechaInicio, fechaFin, tipoOperacion, idUsuario, modulo, entidadAfectada);
        }
    }
}
