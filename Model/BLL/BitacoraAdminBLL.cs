using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using System;
using System.Collections.Generic;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para BitacoraAdmin.
    /// Proporciona métodos para registrar y consultar eventos de administrador.
    /// </summary>
    public class BitacoraAdminBLL
    {
        private readonly IBitacoraAdminRepository _repository;

        public BitacoraAdminBLL(IBitacoraAdminRepository repository)
        {
            _repository = repository;
        }

        public BitacoraAdminBLL() : this(new BitacoraAdminRepository()) { }

        /// <summary>
        /// Registra un evento de error en la bitácora de administrador
        /// </summary>
        public void RegistrarError(string modulo, string accion, string detalle, Guid? idUsuario = null,
            string nombreUsuario = null, string criticidad = "Alta")
        {
            var registro = new BitacoraAdmin
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoEvento = "Error",
                Modulo = modulo,
                Accion = accion,
                Detalle = detalle,
                Criticidad = criticidad
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra un evento de seguridad en la bitácora de administrador
        /// </summary>
        public void RegistrarEventoSeguridad(string modulo, string accion, string detalle, Guid? idUsuario = null,
            string nombreUsuario = null, string criticidad = "Alta", string direccionIP = null)
        {
            var registro = new BitacoraAdmin
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoEvento = "Seguridad",
                Modulo = modulo,
                Accion = accion,
                Detalle = detalle,
                Criticidad = criticidad,
                DireccionIP = direccionIP
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra un cambio crítico en datos importantes
        /// </summary>
        public void RegistrarCambioCritico(string modulo, string accion, string detalle, Guid? idUsuario = null,
            string nombreUsuario = null, string criticidad = "Critica")
        {
            var registro = new BitacoraAdmin
            {
                IdUsuario = idUsuario,
                NombreUsuario = nombreUsuario,
                TipoEvento = "CambioCritico",
                Modulo = modulo,
                Accion = accion,
                Detalle = detalle,
                Criticidad = criticidad
            };

            _repository.Registrar(registro);
        }

        /// <summary>
        /// Registra un evento genérico en la bitácora de administrador
        /// </summary>
        public void RegistrarEvento(BitacoraAdmin registro)
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
        public List<BitacoraAdmin> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        public List<BitacoraAdmin> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");

            return _repository.ObtenerPorFechas(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Obtiene registros filtrados por tipo de evento
        /// </summary>
        public List<BitacoraAdmin> ObtenerPorTipoEvento(string tipoEvento)
        {
            if (string.IsNullOrWhiteSpace(tipoEvento))
                throw new ArgumentException("El tipo de evento no puede estar vacío");

            return _repository.ObtenerPorTipoEvento(tipoEvento);
        }

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        public List<BitacoraAdmin> ObtenerPorUsuario(Guid idUsuario)
        {
            if (idUsuario == Guid.Empty)
                throw new ArgumentException("El ID de usuario no puede estar vacío");

            return _repository.ObtenerPorUsuario(idUsuario);
        }

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        public List<BitacoraAdmin> ObtenerPorModulo(string modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo))
                throw new ArgumentException("El módulo no puede estar vacío");

            return _repository.ObtenerPorModulo(modulo);
        }

        /// <summary>
        /// Obtiene registros filtrados por criticidad
        /// </summary>
        public List<BitacoraAdmin> ObtenerPorCriticidad(string criticidad)
        {
            if (string.IsNullOrWhiteSpace(criticidad))
                throw new ArgumentException("La criticidad no puede estar vacía");

            return _repository.ObtenerPorCriticidad(criticidad);
        }

        /// <summary>
        /// Obtiene registros con múltiples filtros opcionales
        /// </summary>
        public List<BitacoraAdmin> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string criticidad = null)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");

            return _repository.ObtenerConFiltros(fechaInicio, fechaFin, tipoEvento, idUsuario, modulo, criticidad);
        }
    }
}
