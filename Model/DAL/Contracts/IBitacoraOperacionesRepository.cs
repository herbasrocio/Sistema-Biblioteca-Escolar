using DomainModel;
using System;
using System.Collections.Generic;

namespace DAL.Contracts
{
    /// <summary>
    /// Interfaz para el repositorio de BitacoraOperaciones.
    /// Maneja operaciones CRUD para registros de auditoría de operaciones.
    /// </summary>
    public interface IBitacoraOperacionesRepository
    {
        /// <summary>
        /// Registra un nuevo evento en la bitácora de operaciones
        /// </summary>
        void Registrar(BitacoraOperaciones registro);

        /// <summary>
        /// Obtiene todos los registros de la bitácora
        /// </summary>
        List<BitacoraOperaciones> ObtenerTodos();

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        List<BitacoraOperaciones> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene registros filtrados por tipo de operación
        /// </summary>
        List<BitacoraOperaciones> ObtenerPorTipoOperacion(string tipoOperacion);

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        List<BitacoraOperaciones> ObtenerPorUsuario(Guid idUsuario);

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        List<BitacoraOperaciones> ObtenerPorModulo(string modulo);

        /// <summary>
        /// Obtiene registros filtrados por entidad afectada
        /// </summary>
        List<BitacoraOperaciones> ObtenerPorEntidad(string entidadAfectada, int? idEntidad = null);

        /// <summary>
        /// Obtiene registros con múltiples filtros
        /// </summary>
        List<BitacoraOperaciones> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoOperacion = null, Guid? idUsuario = null, string modulo = null, string entidadAfectada = null);
    }
}
