using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    public interface IMaterialRepository : IGenericRepository<Material>
    {
        List<Material> BuscarPorFiltros(string titulo, string autor, string tipo);
        Material ObtenerPorId(Guid idMaterial);
    }
}
