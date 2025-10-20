using System;
using System.Collections.Generic;
using DomainModel;
using DomainModel.Enums;

namespace DAL.Contracts
{
    public interface IEjemplarRepository : IGenericRepository<Ejemplar>
    {
        /// <summary>
        /// Obtiene todos los ejemplares de un material específico
        /// </summary>
        List<Ejemplar> ObtenerPorMaterial(Guid idMaterial);

        /// <summary>
        /// Obtiene un ejemplar específico por su ID
        /// </summary>
        Ejemplar ObtenerPorId(Guid idEjemplar);

        /// <summary>
        /// Obtiene ejemplares por estado
        /// </summary>
        List<Ejemplar> ObtenerPorEstado(EstadoMaterial estado);

        /// <summary>
        /// Obtiene un ejemplar por código de ejemplar
        /// </summary>
        Ejemplar ObtenerPorCodigoEjemplar(string codigoEjemplar);

        /// <summary>
        /// Cuenta ejemplares disponibles de un material
        /// </summary>
        int ContarDisponiblesPorMaterial(Guid idMaterial);

        /// <summary>
        /// Actualiza el estado de un ejemplar
        /// </summary>
        void ActualizarEstado(Guid idEjemplar, EstadoMaterial nuevoEstado);
    }
}
