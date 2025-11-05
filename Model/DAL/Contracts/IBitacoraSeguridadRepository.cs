using DomainModel;
using System;
using System.Collections.Generic;

namespace DAL.Contracts
{
    /// <summary>
    /// Interfaz para el repositorio de BitacoraSeguridad.
    /// Maneja operaciones CRUD para registros de auditoría de administrador.
    /// </summary>
    public interface IBitacoraSeguridadRepository
    {
        /// <summary>
        /// Registra un nuevo evento en la bitácora de administrador
        /// </summary>
        void Registrar(BitacoraSeguridad registro);

        /// <summary>
        /// Obtiene todos los registros de la bitácora
        /// </summary>
        List<BitacoraSeguridad> ObtenerTodos();

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        List<BitacoraSeguridad> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene registros filtrados por tipo de evento
        /// </summary>
        List<BitacoraSeguridad> ObtenerPorTipoEvento(string tipoEvento);

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        List<BitacoraSeguridad> ObtenerPorUsuario(Guid idUsuario);

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        List<BitacoraSeguridad> ObtenerPorModulo(string modulo);

        /// <summary>
        /// Obtiene registros filtrados por gravedad
        /// </summary>
        List<BitacoraSeguridad> ObtenerPorGravedad(string gravedad);

        /// <summary>
        /// Obtiene registros con múltiples filtros
        /// </summary>
        List<BitacoraSeguridad> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string gravedad = null);
    }
}
