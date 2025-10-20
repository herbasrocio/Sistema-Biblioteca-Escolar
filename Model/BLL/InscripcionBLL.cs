using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using DomainModel.Exceptions;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para gestión de Inscripciones y Promoción de Alumnos
    /// </summary>
    public class InscripcionBLL
    {
        private readonly IInscripcionRepository _inscripcionRepository;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        public InscripcionBLL(IInscripcionRepository inscripcionRepository)
        {
            _inscripcionRepository = inscripcionRepository ?? throw new ArgumentNullException(nameof(inscripcionRepository));
        }

        /// <summary>
        /// Constructor sin parámetros
        /// </summary>
        public InscripcionBLL() : this(new InscripcionRepository()) { }

        /// <summary>
        /// Obtiene la inscripción activa de un alumno para el año actual
        /// </summary>
        public Inscripcion ObtenerInscripcionActiva(Guid idAlumno)
        {
            return ObtenerInscripcionActiva(idAlumno, DateTime.Now.Year);
        }

        /// <summary>
        /// Obtiene la inscripción activa de un alumno para un año específico
        /// </summary>
        public Inscripcion ObtenerInscripcionActiva(Guid idAlumno, int anioLectivo)
        {
            try
            {
                return _inscripcionRepository.ObtenerInscripcionActiva(idAlumno, anioLectivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la inscripción activa", ex);
            }
        }

        /// <summary>
        /// Obtiene el historial completo de inscripciones de un alumno
        /// </summary>
        public List<Inscripcion> ObtenerHistorialAlumno(Guid idAlumno)
        {
            try
            {
                return _inscripcionRepository.ObtenerPorAlumno(idAlumno);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el historial del alumno", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las inscripciones de un grado y división
        /// </summary>
        public List<Inscripcion> ObtenerInscripcionesPorGrado(int anioLectivo, string grado, string division = null)
        {
            try
            {
                ValidationBLL.ValidarCampoRequerido(grado, "Grado");
                return _inscripcionRepository.ObtenerPorGradoDivision(anioLectivo, grado, division);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener inscripciones por grado", ex);
            }
        }

        /// <summary>
        /// Inscribe un alumno en un año lectivo
        /// </summary>
        public void InscribirAlumno(Guid idAlumno, int anioLectivo, string grado, string division)
        {
            try
            {
                // Validar que el alumno no tenga ya una inscripción activa para ese año
                var inscripcionExistente = _inscripcionRepository.ObtenerInscripcionActiva(idAlumno, anioLectivo);
                if (inscripcionExistente != null)
                {
                    throw new ValidacionException($"El alumno ya tiene una inscripción activa para el año {anioLectivo}");
                }

                // Validar datos
                ValidationBLL.ValidarCampoRequerido(grado, "Grado");

                // Crear nueva inscripción
                Inscripcion nuevaInscripcion = new Inscripcion
                {
                    IdAlumno = idAlumno,
                    AnioLectivo = anioLectivo,
                    Grado = grado,
                    Division = division,
                    Estado = "Activo"
                };

                _inscripcionRepository.Add(nuevaInscripcion);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al inscribir al alumno", ex);
            }
        }

        /// <summary>
        /// Actualiza una inscripción existente
        /// </summary>
        public void ActualizarInscripcion(Inscripcion inscripcion)
        {
            try
            {
                if (inscripcion == null)
                    throw new ValidacionException("La inscripción no puede ser nula");

                ValidationBLL.ValidarCampoRequerido(inscripcion.Grado, "Grado");

                _inscripcionRepository.Update(inscripcion);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la inscripción", ex);
            }
        }

        /// <summary>
        /// Promociona alumnos de un grado específico al siguiente grado
        /// </summary>
        public ResultadoPromocion PromocionarAlumnosPorGrado(
            int anioActual,
            int anioSiguiente,
            string gradoActual,
            string divisionActual,
            string gradoNuevo,
            string divisionNueva = null)
        {
            try
            {
                ValidationBLL.ValidarCampoRequerido(gradoActual, "Grado Actual");
                ValidationBLL.ValidarCampoRequerido(gradoNuevo, "Grado Nuevo");

                if (anioSiguiente <= anioActual)
                    throw new ValidacionException("El año siguiente debe ser mayor al año actual");

                ResultadoPromocion resultado = new ResultadoPromocion();

                // 1. Obtener inscripciones del grado actual
                var inscripciones = _inscripcionRepository.ObtenerPorGradoDivision(
                    anioActual, gradoActual, divisionActual);

                if (inscripciones.Count == 0)
                {
                    throw new ValidacionException($"No se encontraron alumnos en {gradoActual}° {divisionActual ?? ""}");
                }

                // 2. Finalizar inscripciones del año actual
                foreach (var inscripcion in inscripciones)
                {
                    _inscripcionRepository.FinalizarInscripcion(inscripcion.IdInscripcion);
                    resultado.AlumnosFinalizados++;
                }

                // 3. Crear nuevas inscripciones para el año siguiente
                foreach (var inscripcion in inscripciones)
                {
                    // Verificar que no exista ya inscripción para el año siguiente
                    var inscripcionExistente = _inscripcionRepository.ObtenerInscripcionActiva(
                        inscripcion.IdAlumno, anioSiguiente);

                    if (inscripcionExistente == null)
                    {
                        Inscripcion nuevaInscripcion = new Inscripcion
                        {
                            IdAlumno = inscripcion.IdAlumno,
                            AnioLectivo = anioSiguiente,
                            Grado = gradoNuevo,
                            Division = divisionNueva ?? inscripcion.Division,
                            Estado = "Activo"
                        };

                        _inscripcionRepository.Add(nuevaInscripcion);
                        resultado.AlumnosPromovidos++;
                    }
                }

                resultado.Exitoso = true;
                resultado.Mensaje = $"Se promovieron {resultado.AlumnosPromovidos} alumnos de {gradoActual}° a {gradoNuevo}°";

                return resultado;
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al promocionar alumnos", ex);
            }
        }

        /// <summary>
        /// Promociona todos los alumnos según mapeo automático de grados
        /// </summary>
        public ResultadoPromocion PromocionarTodosLosAlumnos(int anioActual, int anioSiguiente)
        {
            try
            {
                if (anioSiguiente <= anioActual)
                    throw new ValidacionException("El año siguiente debe ser mayor al año actual");

                ResultadoPromocion resultado = new ResultadoPromocion();

                // Mapeo de grados
                Dictionary<string, string> mapeoGrados = new Dictionary<string, string>
                {
                    { "1", "2" },
                    { "2", "3" },
                    { "3", "4" },
                    { "4", "5" },
                    { "5", "6" },
                    { "6", "7" },
                    { "7", "EGRESADO" }
                };

                // Obtener todas las inscripciones activas del año actual
                var todasInscripciones = _inscripcionRepository.ObtenerPorAnioLectivo(anioActual)
                    .Where(i => i.Estado == "Activo")
                    .ToList();

                if (todasInscripciones.Count == 0)
                {
                    throw new ValidacionException($"No se encontraron inscripciones activas para el año {anioActual}");
                }

                // Finalizar todas las inscripciones del año actual
                int finalizados = _inscripcionRepository.FinalizarInscripcionesPorAnio(anioActual);
                resultado.AlumnosFinalizados = finalizados;

                // Promocionar según mapeo
                foreach (var inscripcion in todasInscripciones)
                {
                    if (mapeoGrados.ContainsKey(inscripcion.Grado))
                    {
                        string gradoNuevo = mapeoGrados[inscripcion.Grado];

                        if (gradoNuevo == "EGRESADO")
                        {
                            resultado.Egresados++;
                        }
                        else
                        {
                            // Verificar que no exista ya inscripción
                            var existente = _inscripcionRepository.ObtenerInscripcionActiva(
                                inscripcion.IdAlumno, anioSiguiente);

                            if (existente == null)
                            {
                                Inscripcion nuevaInscripcion = new Inscripcion
                                {
                                    IdAlumno = inscripcion.IdAlumno,
                                    AnioLectivo = anioSiguiente,
                                    Grado = gradoNuevo,
                                    Division = inscripcion.Division,
                                    Estado = "Activo"
                                };

                                _inscripcionRepository.Add(nuevaInscripcion);
                                resultado.AlumnosPromovidos++;
                            }
                        }
                    }
                }

                resultado.Exitoso = true;
                resultado.Mensaje = $"Promoción completada. {resultado.AlumnosPromovidos} alumnos promovidos, {resultado.Egresados} egresados";

                return resultado;
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la promoción masiva", ex);
            }
        }

        /// <summary>
        /// Obtiene estadísticas de inscripciones por grado para un año
        /// </summary>
        public List<EstadisticaGrado> ObtenerEstadisticasPorAnio(int anioLectivo)
        {
            try
            {
                var inscripciones = _inscripcionRepository.ObtenerPorAnioLectivo(anioLectivo)
                    .Where(i => i.Estado == "Activo")
                    .ToList();

                var estadisticas = inscripciones
                    .GroupBy(i => new { i.Grado, i.Division })
                    .Select(g => new EstadisticaGrado
                    {
                        Grado = g.Key.Grado,
                        Division = g.Key.Division,
                        CantidadAlumnos = g.Count()
                    })
                    .OrderBy(e => e.Grado)
                    .ThenBy(e => e.Division)
                    .ToList();

                return estadisticas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener estadísticas", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los años lectivos disponibles en el sistema
        /// </summary>
        public List<int> ObtenerAniosLectivosDisponibles()
        {
            try
            {
                var todasInscripciones = _inscripcionRepository.GetAll();
                var anios = todasInscripciones
                    .Select(i => i.AnioLectivo)
                    .Distinct()
                    .OrderByDescending(a => a)
                    .ToList();

                return anios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener años lectivos disponibles", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las inscripciones de un año lectivo
        /// </summary>
        public List<Inscripcion> ObtenerInscripcionesPorAnio(int anioLectivo)
        {
            try
            {
                return _inscripcionRepository.ObtenerPorAnioLectivo(anioLectivo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener inscripciones del año {anioLectivo}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las inscripciones (historial completo) de un alumno
        /// </summary>
        public List<Inscripcion> ObtenerInscripcionesPorAlumno(Guid idAlumno)
        {
            try
            {
                return _inscripcionRepository.ObtenerPorAlumno(idAlumno);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener inscripciones del alumno", ex);
            }
        }
    }

    /// <summary>
    /// Resultado de una operación de promoción
    /// </summary>
    public class ResultadoPromocion
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int AlumnosFinalizados { get; set; }
        public int AlumnosPromovidos { get; set; }
        public int Egresados { get; set; }
    }

    /// <summary>
    /// Estadística de alumnos por grado
    /// </summary>
    public class EstadisticaGrado
    {
        public string Grado { get; set; }
        public string Division { get; set; }
        public int CantidadAlumnos { get; set; }

        public override string ToString()
        {
            return $"{Grado}° {Division}: {CantidadAlumnos} alumnos";
        }
    }
}
