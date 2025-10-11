using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Contrato para el repositorio de Inscripciones
    /// </summary>
    public interface IInscripcionRepository : IGenericRepository<Inscripcion>
    {
        /// <summary>
        /// Obtiene una inscripción por su ID
        /// </summary>
        Inscripcion ObtenerPorId(Guid idInscripcion);

        /// <summary>
        /// Obtiene la inscripción activa de un alumno para un año lectivo
        /// </summary>
        Inscripcion ObtenerInscripcionActiva(Guid idAlumno, int anioLectivo);

        /// <summary>
        /// Obtiene todas las inscripciones de un alumno (historial)
        /// </summary>
        List<Inscripcion> ObtenerPorAlumno(Guid idAlumno);

        /// <summary>
        /// Obtiene todas las inscripciones de un año lectivo, grado y división
        /// </summary>
        List<Inscripcion> ObtenerPorGradoDivision(int anioLectivo, string grado, string division = null);

        /// <summary>
        /// Obtiene todas las inscripciones de un año lectivo
        /// </summary>
        List<Inscripcion> ObtenerPorAnioLectivo(int anioLectivo);

        /// <summary>
        /// Finaliza una inscripción (cambia estado a Finalizado)
        /// </summary>
        void FinalizarInscripcion(Guid idInscripcion);

        /// <summary>
        /// Finaliza todas las inscripciones activas de un año lectivo
        /// </summary>
        int FinalizarInscripcionesPorAnio(int anioLectivo);
    }
}
