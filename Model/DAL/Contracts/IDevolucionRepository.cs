using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    public interface IDevolucionRepository : IGenericRepository<Devolucion>
    {
        Devolucion ObtenerPorId(Guid idDevolucion);
        Devolucion ObtenerPorPrestamo(Guid idPrestamo);
        List<Devolucion> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin);
        List<Devolucion> ObtenerDevolucionesAtrasadas();
    }
}
