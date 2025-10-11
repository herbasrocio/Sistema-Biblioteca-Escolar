using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    public interface IAlumnoRepository : IGenericRepository<Alumno>
    {
        Alumno ObtenerPorId(Guid idAlumno);
        Alumno ObtenerPorDNI(string dni);
        List<Alumno> BuscarPorNombre(string nombre);
        List<Alumno> BuscarPorGradoDivision(string grado, string division);
    }
}
