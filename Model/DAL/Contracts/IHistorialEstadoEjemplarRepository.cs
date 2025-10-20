using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    public interface IHistorialEstadoEjemplarRepository
    {
        /// <summary>
        /// Registra un cambio de estado en el historial
        /// </summary>
        void RegistrarCambio(HistorialEstadoEjemplar historial);

        /// <summary>
        /// Obtiene todo el historial de cambios de un ejemplar
        /// </summary>
        List<HistorialEstadoEjemplar> ObtenerHistorialPorEjemplar(Guid idEjemplar);

        /// <summary>
        /// Obtiene el historial de cambios filtrado por tipo
        /// </summary>
        List<HistorialEstadoEjemplar> ObtenerHistorialPorTipo(Guid idEjemplar, TipoCambioEstado tipoCambio);

        /// <summary>
        /// Obtiene el historial de cambios en un rango de fechas
        /// </summary>
        List<HistorialEstadoEjemplar> ObtenerHistorialPorFechas(Guid idEjemplar, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene el último cambio de estado de un ejemplar
        /// </summary>
        HistorialEstadoEjemplar ObtenerUltimoCambio(Guid idEjemplar);

        /// <summary>
        /// Obtiene todos los registros de historial
        /// </summary>
        List<HistorialEstadoEjemplar> ObtenerTodos();
    }
}
