using DomainModel;
using System;
using System.Collections.Generic;

namespace DAL.Contracts
{
    /// <summary>
    /// Interfaz para el repositorio de BitacoraBibliotecario.
    /// Maneja operaciones CRUD para registros de auditoría de bibliotecarios.
    /// </summary>
    public interface IBitacoraBibliotecarioRepository
    {
        /// <summary>
        /// Registra un nuevo evento en la bitácora de bibliotecario
        /// </summary>
        void Registrar(BitacoraBibliotecario registro);

        /// <summary>
        /// Obtiene todos los registros de la bitácora
        /// </summary>
        List<BitacoraBibliotecario> ObtenerTodos();

        /// <summary>
        /// Obtiene registros filtrados por rango de fechas
        /// </summary>
        List<BitacoraBibliotecario> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene registros filtrados por tipo de operación
        /// </summary>
        List<BitacoraBibliotecario> ObtenerPorTipoOperacion(string tipoOperacion);

        /// <summary>
        /// Obtiene registros filtrados por usuario
        /// </summary>
        List<BitacoraBibliotecario> ObtenerPorUsuario(Guid idUsuario);

        /// <summary>
        /// Obtiene registros filtrados por módulo
        /// </summary>
        List<BitacoraBibliotecario> ObtenerPorModulo(string modulo);

        /// <summary>
        /// Obtiene registros filtrados por entidad afectada
        /// </summary>
        List<BitacoraBibliotecario> ObtenerPorEntidad(string entidadAfectada, int? idEntidad = null);

        /// <summary>
        /// Obtiene registros con múltiples filtros
        /// </summary>
        List<BitacoraBibliotecario> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
            string tipoOperacion = null, Guid? idUsuario = null, string modulo = null, string entidadAfectada = null);
    }
}
