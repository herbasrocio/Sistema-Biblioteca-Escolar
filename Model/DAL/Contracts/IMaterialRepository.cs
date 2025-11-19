using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Define el contrato para el repositorio de materiales bibliográficos.
    /// Proporciona operaciones específicas de acceso a datos para la entidad <see cref="Material"/>.
    /// </summary>
    /// <remarks>
    /// Esta interfaz extiende <see cref="IGenericRepository{Material}"/> agregando
    /// operaciones especializadas para búsqueda y consulta de materiales.
    /// </remarks>
    public interface IMaterialRepository : IGenericRepository<Material>
    {
        /// <summary>
        /// Busca materiales aplicando filtros opcionales de título, autor y tipo.
        /// </summary>
        /// <param name="titulo">Título o parte del título a buscar. Puede ser <c>null</c> para omitir este filtro.</param>
        /// <param name="autor">Autor o parte del autor a buscar. Puede ser <c>null</c> para omitir este filtro.</param>
        /// <param name="tipo">Tipo de material. Puede ser <c>null</c> o "Todos" para omitir este filtro.</param>
        /// <returns>
        /// Lista de materiales que coinciden con los criterios de búsqueda.
        /// Las cantidades (CantidadTotal, CantidadDisponible) se calculan dinámicamente.
        /// </returns>
        List<Material> BuscarPorFiltros(string titulo, string autor, string tipo);

        /// <summary>
        /// Obtiene un material específico por su identificador único.
        /// </summary>
        /// <param name="idMaterial">Identificador único del material.</param>
        /// <returns>
        /// El objeto <see cref="Material"/> con las cantidades calculadas dinámicamente,
        /// o <c>null</c> si no se encuentra.
        /// </returns>
        Material ObtenerPorId(Guid idMaterial);
    }
}
