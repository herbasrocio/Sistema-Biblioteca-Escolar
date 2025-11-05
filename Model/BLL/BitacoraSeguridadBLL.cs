using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using System;
using System.Collections.Generic;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para BitacoraSeguridad.
    /// Proporciona métodos para registrar y consultar eventos de seguridad.
    /// </summary>
    public class BitacoraSeguridadBLL
    {
        private readonly IBitacoraSeguridadRepository _repository;

        public BitacoraSeguridadBLL(IBitacoraSeguridadRepository repository)
        {
            _repository = repository;
        }

        public BitacoraSeguridadBLL() : this(new BitacoraSeguridadRepository()) { }

        /// <summary>
        /// Registra un evento de error en la bitácora de seguridad
        /// </summary>
        public void RegistrarError(string modulo, string accion, string detalle, Guid? idUsuario = null,
            string nombreUsuario = null, string gravedad = "Alto")
        {
            var registro = new BitacoraSeguridad
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoEvento = "Error",
                Modulo = modulo,
                Accion = accion,
                Detalle = detalle,
                Gravedad = gravedad
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra un evento de seguridad en la bitácora de seguridad
        /// </summary>
        public void RegistrarEventoSeguridad(string modulo, string accion, string detalle, Guid? idUsuario = null,
            string nombreUsuario = null, string gravedad = "Alto", string direccionIP = null)
        {
            var registro = new BitacoraSeguridad
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoEvento = "Seguridad",
                Modulo = modulo,
                Accion = accion,
                Detalle = detalle,
                Gravedad = gravedad,
                DireccionIP = direccionIP
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra un cambio crítico en datos importantes
        /// </summary>
        public void RegistrarCambioCritico(string modulo, string accion, string detalle, Guid? idUsuario = null,
            string nombreUsuario = null, string gravedad = "Alto")
        {
            var registro = new BitacoraSeguridad
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoEvento = "CambioCritico",
                Modulo = modulo,
                Accion = accion,
                Detalle = detalle,
                Gravedad = gravedad
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra un evento genérico en la bitácora de seguridad
        /// </summary>
        public void RegistrarEvento(BitacoraSeguridad registro)
        {
            if (registro == null)
                throw new ArgumentNullException(nameof(registro), "El registro de bitácora no puede ser nulo");

            if (string.IsNullOrWhiteSpace(registro.TipoEvento))
                throw new ArgumentException("El tipo de evento es obligatorio");

            if (string.IsNullOrWhiteSpace(registro.Modulo))
                throw new ArgumentException("El módulo es obligatorio");

            if (string.IsNullOrWhiteSpace(registro.Accion))
                throw new ArgumentException("La acción es obligatoria");

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Obtiene todos los registros de la bitácora
        /// </summary>
        public List<BitacoraSeguridad> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        public List<BitacoraSeguridad> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");

            return _repository.ObtenerPorFechas(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Obtiene registros filtrados por tipo de evento
        /// </summary>
        public List<BitacoraSeguridad> ObtenerPorTipoEvento(string tipoEvento)
        {
            if (string.IsNullOrWhiteSpace(tipoEvento))
                throw new ArgumentException("El tipo de evento no puede estar vacío");

            return _repository.ObtenerPorTipoEvento(tipoEvento);
        }

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        public List<BitacoraSeguridad> ObtenerPorUsuario(Guid idUsuario)
        {
            if (idUsuario == Guid.Empty)
                throw new ArgumentException("El ID de usuario no puede estar vacío");

            return _repository.ObtenerPorUsuario(idUsuario);
        }

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        public List<BitacoraSeguridad> ObtenerPorModulo(string modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo))
                throw new ArgumentException("El módulo no puede estar vacío");

            return _repository.ObtenerPorModulo(modulo);
        }

        /// <summary>
        /// Obtiene registros filtrados por gravedad
        /// </summary>
        public List<BitacoraSeguridad> ObtenerPorGravedad(string gravedad)
        {
            if (string.IsNullOrWhiteSpace(gravedad))
                throw new ArgumentException("La gravedad no puede estar vacía");

            return _repository.ObtenerPorGravedad(gravedad);
        }

        /// <summary>
        /// Obtiene registros con múltiples filtros opcionales
        /// </summary>
        public List<BitacoraSeguridad> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string gravedad = null)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");

            return _repository.ObtenerConFiltros(fechaInicio, fechaFin, tipoEvento, idUsuario, modulo, gravedad);
        }
    }
}
