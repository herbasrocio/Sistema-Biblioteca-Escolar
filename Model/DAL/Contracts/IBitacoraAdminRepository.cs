using DomainModel;
using System;
using System.Collections.Generic;

namespace DAL.Contracts
{
    /// <summary>
    /// Interfaz para el repositorio de BitacoraAdmin.
    /// Maneja operaciones CRUD para registros de auditoría de administrador.
    /// </summary>
    public interface IBitacoraAdminRepository
    {
        /// <summary>
        /// Registra un nuevo evento en la bitácora de administrador
        /// </summary>
        void Registrar(BitacoraAdmin registro);

        /// <summary>
        /// Obtiene todos los registros de la bitácora
        /// </summary>
        List<BitacoraAdmin> ObtenerTodos();

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        List<BitacoraAdmin> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene registros filtrados por tipo de evento
        /// </summary>
        List<BitacoraAdmin> ObtenerPorTipoEvento(string tipoEvento);

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        List<BitacoraAdmin> ObtenerPorUsuario(Guid idUsuario);

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        List<BitacoraAdmin> ObtenerPorModulo(string modulo);

        /// <summary>
        /// Obtiene registros filtrados por criticidad
        /// </summary>
        List<BitacoraAdmin> ObtenerPorCriticidad(string criticidad);

        /// <summary>
        /// Obtiene registros con múltiples filtros
        /// </summary>
        List<BitacoraAdmin> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string criticidad = null);
    }
}
