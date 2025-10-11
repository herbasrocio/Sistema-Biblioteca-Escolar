using System;
using System.Collections.Generic;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using DomainModel.Exceptions;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para la gestión de Alumnos
    /// </summary>
    public class AlumnoBLL
    {
        private readonly IAlumnoRepository _alumnoRepository;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        /// <param name="alumnoRepository">Repositorio de alumnos</param>
        public AlumnoBLL(IAlumnoRepository alumnoRepository)
        {
            _alumnoRepository = alumnoRepository ?? throw new ArgumentNullException(nameof(alumnoRepository));
        }

        /// <summary>
        /// Constructor sin parámetros (crea la dependencia internamente)
        /// </summary>
        public AlumnoBLL() : this(new AlumnoRepository()) { }

        /// <summary>
        /// Obtiene todos los alumnos activos
        /// </summary>
        /// <returns>Lista de alumnos activos</returns>
        public List<Alumno> ObtenerTodosAlumnos()
        {
            try
            {
                return _alumnoRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de alumnos", ex);
            }
        }

        /// <summary>
        /// Obtiene un alumno por su ID
        /// </summary>
        /// <param name="idAlumno">ID del alumno</param>
        /// <returns>Alumno encontrado o null</returns>
        public Alumno ObtenerAlumnoPorId(Guid idAlumno)
        {
            try
            {
                if (idAlumno == Guid.Empty)
                {
                    throw new ValidacionException("El ID del alumno no es válido");
                }

                return _alumnoRepository.ObtenerPorId(idAlumno);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el alumno", ex);
            }
        }

        /// <summary>
        /// Obtiene un alumno por su DNI
        /// </summary>
        /// <param name="dni">DNI del alumno</param>
        /// <returns>Alumno encontrado o null</returns>
        public Alumno ObtenerAlumnoPorDNI(string dni)
        {
            try
            {
                ValidationBLL.ValidarCampoRequerido(dni, "DNI");
                return _alumnoRepository.ObtenerPorDNI(dni);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el alumno por DNI", ex);
            }
        }

        /// <summary>
        /// Busca alumnos por nombre o apellido
        /// </summary>
        /// <param name="nombre">Texto a buscar en nombre o apellido</param>
        /// <returns>Lista de alumnos que coinciden</returns>
        public List<Alumno> BuscarAlumnos(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return ObtenerTodosAlumnos();
                }

                return _alumnoRepository.BuscarPorNombre(nombre);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar alumnos", ex);
            }
        }

        /// <summary>
        /// Busca alumnos por grado y división
        /// </summary>
        /// <param name="grado">Grado a buscar (opcional)</param>
        /// <param name="division">División a buscar (opcional)</param>
        /// <returns>Lista de alumnos que coinciden</returns>
        public List<Alumno> BuscarPorGradoDivision(string grado, string division)
        {
            try
            {
                return _alumnoRepository.BuscarPorGradoDivision(grado, division);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar alumnos por grado y división", ex);
            }
        }

        /// <summary>
        /// Guarda un nuevo alumno
        /// </summary>
        /// <param name="alumno">Alumno a guardar</param>
        /// <exception cref="ValidacionException">Si los datos son inválidos</exception>
        public void GuardarAlumno(Alumno alumno)
        {
            try
            {
                // Validar todos los campos del alumno
                ValidationBLL.ValidarAlumno(alumno);

                // Validar DNI único
                var alumnoExistente = _alumnoRepository.ObtenerPorDNI(alumno.DNI);
                if (alumnoExistente != null)
                {
                    throw new ValidacionException($"Ya existe un alumno registrado con el DNI {alumno.DNI}");
                }

                // Guardar el alumno
                _alumnoRepository.Add(alumno);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el alumno", ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un alumno existente
        /// </summary>
        /// <param name="alumno">Alumno con datos actualizados</param>
        /// <exception cref="ValidacionException">Si los datos son inválidos</exception>
        public void ActualizarAlumno(Alumno alumno)
        {
            try
            {
                // Validar todos los campos del alumno
                ValidationBLL.ValidarAlumno(alumno);

                // Validar que el alumno exista
                var alumnoActual = _alumnoRepository.ObtenerPorId(alumno.IdAlumno);
                if (alumnoActual == null)
                {
                    throw new ValidacionException("El alumno que intenta actualizar no existe");
                }

                // Validar DNI único (excepto el mismo alumno)
                var alumnoExistente = _alumnoRepository.ObtenerPorDNI(alumno.DNI);
                if (alumnoExistente != null && alumnoExistente.IdAlumno != alumno.IdAlumno)
                {
                    throw new ValidacionException($"Ya existe otro alumno registrado con el DNI {alumno.DNI}");
                }

                // Actualizar el alumno
                _alumnoRepository.Update(alumno);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el alumno", ex);
            }
        }

        /// <summary>
        /// Elimina lógicamente un alumno (marca como inactivo)
        /// </summary>
        /// <param name="alumno">Alumno a eliminar</param>
        /// <exception cref="ValidacionException">Si el alumno tiene préstamos activos</exception>
        public void EliminarAlumno(Alumno alumno)
        {
            try
            {
                if (alumno == null)
                {
                    throw new ValidacionException("El alumno a eliminar no puede ser nulo");
                }

                // Validar que el alumno exista
                var alumnoActual = _alumnoRepository.ObtenerPorId(alumno.IdAlumno);
                if (alumnoActual == null)
                {
                    throw new ValidacionException("El alumno que intenta eliminar no existe");
                }

                // TODO: Validar que no tenga préstamos activos cuando exista PrestamoBLL
                // var prestamosActivos = prestamoBLL.ObtenerPrestamosActivosPorAlumno(alumno.IdAlumno);
                // if (prestamosActivos.Count > 0)
                // {
                //     throw new ValidacionException($"No se puede eliminar el alumno porque tiene {prestamosActivos.Count} préstamo(s) activo(s)");
                // }

                // Eliminar (marca como inactivo)
                _alumnoRepository.Delete(alumno);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el alumno", ex);
            }
        }

        /// <summary>
        /// Verifica si un alumno puede ser eliminado (no tiene préstamos activos)
        /// </summary>
        /// <param name="idAlumno">ID del alumno</param>
        /// <returns>True si puede ser eliminado, false en caso contrario</returns>
        public bool PuedeEliminarAlumno(Guid idAlumno)
        {
            try
            {
                // TODO: Implementar cuando exista PrestamoBLL
                // var prestamosActivos = prestamoBLL.ObtenerPrestamosActivosPorAlumno(idAlumno);
                // return prestamosActivos.Count == 0;

                return true; // Por ahora siempre retorna true
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar si el alumno puede ser eliminado", ex);
            }
        }
    }
}
