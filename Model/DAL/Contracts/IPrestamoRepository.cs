using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    public interface IPrestamoRepository : IGenericRepository<Prestamo>
    {
        Prestamo ObtenerPorId(Guid idPrestamo);
        List<Prestamo> ObtenerPorAlumno(Guid idAlumno);
        List<Prestamo> ObtenerPorMaterial(Guid idMaterial);
        List<Prestamo> ObtenerActivos();
        List<Prestamo> ObtenerAtrasados();
        List<Prestamo> ObtenerPorEstado(string estado);
        void ActualizarEstado(Guid idPrestamo, string nuevoEstado);
        System.Data.DataTable BuscarPrestamosActivos(string nombreAlumno = null, string tituloMaterial = null, string codigoEjemplar = null);
        void RenovarPrestamo(Guid idPrestamo, DateTime nuevaFechaDevolucion, Guid idUsuario, string observaciones = null);
        List<RenovacionPrestamo> ObtenerRenovacionesPorPrestamo(Guid idPrestamo);
    }
}
