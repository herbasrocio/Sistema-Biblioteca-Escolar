using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    public class PrestamoBLL
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IEjemplarRepository _ejemplarRepository;

        // Constructor con inyección de dependencias
        public PrestamoBLL(IPrestamoRepository prestamoRepository, IMaterialRepository materialRepository, IAlumnoRepository alumnoRepository, IEjemplarRepository ejemplarRepository)
        {
            _prestamoRepository = prestamoRepository ?? throw new ArgumentNullException(nameof(prestamoRepository));
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
            _alumnoRepository = alumnoRepository ?? throw new ArgumentNullException(nameof(alumnoRepository));
            _ejemplarRepository = ejemplarRepository ?? throw new ArgumentNullException(nameof(ejemplarRepository));
        }

        // Constructor sin parámetros
        public PrestamoBLL() : this(new PrestamoRepository(), new MaterialRepository(), new AlumnoRepository(), new EjemplarRepository()) { }

        public List<Prestamo> ObtenerTodosPrestamos()
        {
            return _prestamoRepository.GetAll();
        }

        public Prestamo ObtenerPrestamoPorId(Guid idPrestamo)
        {
            return _prestamoRepository.ObtenerPorId(idPrestamo);
        }

        public List<Prestamo> ObtenerPorAlumno(Guid idAlumno)
        {
            return _prestamoRepository.ObtenerPorAlumno(idAlumno);
        }

        public List<Prestamo> ObtenerPorMaterial(Guid idMaterial)
        {
            return _prestamoRepository.ObtenerPorMaterial(idMaterial);
        }

        public List<Prestamo> ObtenerPrestamosActivos()
        {
            return _prestamoRepository.ObtenerActivos();
        }

        public List<Prestamo> ObtenerPrestamosAtrasados()
        {
            return _prestamoRepository.ObtenerAtrasados();
        }

        public void RegistrarPrestamo(Prestamo prestamo)
        {
            // Validaciones
            if (prestamo.IdMaterial == Guid.Empty)
                throw new Exception("Debe seleccionar un material");

            if (prestamo.IdAlumno == Guid.Empty)
                throw new Exception("Debe seleccionar un alumno");

            if (prestamo.IdUsuario == Guid.Empty)
                throw new Exception("Usuario no válido");

            if (prestamo.FechaDevolucionPrevista <= prestamo.FechaPrestamo)
                throw new Exception("La fecha de devolución prevista debe ser posterior a la fecha de préstamo");

            // Validar que el material existe
            var material = _materialRepository.ObtenerPorId(prestamo.IdMaterial);
            if (material == null)
                throw new Exception("El material no existe");

            // Validar que el alumno existe
            var alumno = _alumnoRepository.ObtenerPorId(prestamo.IdAlumno);
            if (alumno == null)
                throw new Exception("El alumno no existe");

            // Verificar que el alumno no tenga préstamos atrasados
            var prestamosAlumno = _prestamoRepository.ObtenerPorAlumno(prestamo.IdAlumno);
            foreach (var p in prestamosAlumno)
            {
                if (p.EstaAtrasado())
                    throw new Exception($"El alumno {alumno.NombreCompleto} tiene préstamos atrasados. Debe devolverlos antes de realizar un nuevo préstamo.");
            }

            // Validar que se haya seleccionado un ejemplar específico
            if (prestamo.IdEjemplar == Guid.Empty)
                throw new Exception("Debe seleccionar un ejemplar específico del material");

            // Obtener el ejemplar seleccionado y validar que está disponible
            var ejemplarSeleccionado = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);

            if (ejemplarSeleccionado == null)
                throw new Exception("El ejemplar seleccionado no existe");

            if (ejemplarSeleccionado.IdMaterial != prestamo.IdMaterial)
                throw new Exception("El ejemplar seleccionado no corresponde al material elegido");

            if (ejemplarSeleccionado.Estado != DomainModel.Enums.EstadoMaterial.Disponible)
                throw new Exception($"El ejemplar seleccionado no está disponible. Estado actual: {ejemplarSeleccionado.Estado}");

            // Cambiar estado del ejemplar a Prestado
            ejemplarSeleccionado.Estado = DomainModel.Enums.EstadoMaterial.Prestado;
            _ejemplarRepository.Update(ejemplarSeleccionado);

            // Generar GUID para el préstamo si no existe
            if (prestamo.IdPrestamo == Guid.Empty)
                prestamo.IdPrestamo = Guid.NewGuid();

            // Registrar préstamo
            _prestamoRepository.Add(prestamo);

            // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
            // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
        }

        public void ActualizarEstadoPrestamo(Guid idPrestamo, string nuevoEstado)
        {
            var prestamo = _prestamoRepository.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo no existe");

            _prestamoRepository.ActualizarEstado(idPrestamo, nuevoEstado);
        }

        public void MarcarComoDevuelto(Guid idPrestamo)
        {
            var prestamo = _prestamoRepository.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo no existe");

            if (prestamo.Estado == "Devuelto")
                throw new Exception("Este préstamo ya fue devuelto");

            // Cambiar estado del ejemplar a Disponible
            if (prestamo.IdEjemplar != Guid.Empty)
            {
                var ejemplar = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);
                if (ejemplar != null)
                {
                    ejemplar.Estado = DomainModel.Enums.EstadoMaterial.Disponible;
                    _ejemplarRepository.Update(ejemplar);
                }
            }

            // Actualizar estado del préstamo
            _prestamoRepository.ActualizarEstado(idPrestamo, "Devuelto");

            // NOTA: No se actualiza manualmente CantidadDisponible porque ahora se calcula
            // dinámicamente en MaterialRepository basándose en el estado de los ejemplares
        }

        public void ActualizarPrestamosAtrasados()
        {
            // Actualizar automáticamente el estado de préstamos atrasados
            var prestamosActivos = _prestamoRepository.ObtenerActivos();
            foreach (var prestamo in prestamosActivos)
            {
                if (prestamo.EstaAtrasado())
                {
                    _prestamoRepository.ActualizarEstado(prestamo.IdPrestamo, "Atrasado");
                }
            }
        }

        /// <summary>
        /// Obtiene todos los préstamos que están activos o atrasados (pendientes de devolución)
        /// </summary>
        public List<Prestamo> ObtenerPrestamosActivosYVencidos()
        {
            var prestamos = _prestamoRepository.GetAll();
            return prestamos
                .Where(p => p.Estado == "Activo" || p.Estado == "Atrasado")
                .OrderBy(p => p.FechaDevolucionPrevista)
                .ToList();
        }

        /// <summary>
        /// Busca un préstamo activo por código de ejemplar
        /// </summary>
        public Prestamo BuscarPrestamoPorCodigoEjemplar(string codigoEjemplar)
        {
            if (string.IsNullOrWhiteSpace(codigoEjemplar))
                return null;

            var prestamosActivos = ObtenerPrestamosActivosYVencidos();

            foreach (var prestamo in prestamosActivos)
            {
                if (prestamo.IdEjemplar != Guid.Empty)
                {
                    var ejemplar = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);
                    if (ejemplar != null && ejemplar.CodigoEjemplar != null)
                    {
                        if (ejemplar.CodigoEjemplar.Equals(codigoEjemplar, StringComparison.OrdinalIgnoreCase))
                        {
                            // Cargar datos relacionados
                            prestamo.Material = _materialRepository.ObtenerPorId(prestamo.IdMaterial);
                            prestamo.Alumno = _alumnoRepository.ObtenerPorId(prestamo.IdAlumno);
                            prestamo.Ejemplar = ejemplar;
                            return prestamo;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Busca préstamos activos con filtros en tiempo real
        /// </summary>
        /// <param name="nombreAlumno">Nombre del alumno (búsqueda parcial)</param>
        /// <param name="tituloMaterial">Título del material (búsqueda parcial)</param>
        /// <param name="codigoEjemplar">Código del ejemplar (búsqueda parcial)</param>
        /// <returns>DataTable con información completa de préstamos activos</returns>
        public System.Data.DataTable BuscarPrestamosActivos(string nombreAlumno = null, string tituloMaterial = null, string codigoEjemplar = null)
        {
            return _prestamoRepository.BuscarPrestamosActivos(nombreAlumno, tituloMaterial, codigoEjemplar);
        }

        /// <summary>
        /// Renueva un préstamo existente, extendiendo su fecha de devolución
        /// </summary>
        /// <param name="idPrestamo">ID del préstamo a renovar</param>
        /// <param name="diasExtension">Días a extender desde HOY</param>
        /// <param name="idUsuario">ID del usuario que procesa la renovación</param>
        /// <param name="maxRenovaciones">Máximo de renovaciones permitidas (default: 2)</param>
        /// <param name="maxDiasAtraso">Máximo de días de atraso para permitir renovación (default: 7)</param>
        /// <param name="observaciones">Observaciones opcionales</param>
        public void RenovarPrestamo(Guid idPrestamo, int diasExtension, Guid idUsuario, int maxRenovaciones = 2, int maxDiasAtraso = 7, string observaciones = null)
        {
            // Validaciones
            if (idPrestamo == Guid.Empty)
                throw new Exception("ID de préstamo inválido");

            if (idUsuario == Guid.Empty)
                throw new Exception("Usuario no válido");

            if (diasExtension <= 0)
                throw new Exception("La extensión debe ser al menos 1 día");

            // Obtener el préstamo
            var prestamo = _prestamoRepository.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                throw new Exception("El préstamo no existe");

            // Validar que el préstamo esté activo
            if (prestamo.Estado != "Activo" && prestamo.Estado != "Atrasado")
                throw new Exception($"No se puede renovar un préstamo con estado '{prestamo.Estado}'. Solo se pueden renovar préstamos activos.");

            // Validar límite de renovaciones
            if (prestamo.CantidadRenovaciones >= maxRenovaciones)
                throw new Exception($"Este préstamo ya alcanzó el límite máximo de {maxRenovaciones} renovaciones.");

            // Validar que no esté demasiado atrasado
            if (prestamo.Estado == "Atrasado")
            {
                int diasAtraso = Math.Abs((DateTime.Now - prestamo.FechaDevolucionPrevista).Days);
                if (diasAtraso > maxDiasAtraso)
                    throw new Exception($"Este préstamo tiene {diasAtraso} días de atraso. No se puede renovar préstamos con más de {maxDiasAtraso} días de atraso.");
            }

            // Verificar que el alumno no tenga otros préstamos muy atrasados
            var prestamosAlumno = _prestamoRepository.ObtenerPorAlumno(prestamo.IdAlumno);
            foreach (var p in prestamosAlumno)
            {
                if (p.IdPrestamo != idPrestamo && p.Estado == "Atrasado")
                {
                    int diasAtraso = Math.Abs((DateTime.Now - p.FechaDevolucionPrevista).Days);
                    if (diasAtraso > maxDiasAtraso)
                    {
                        var alumno = _alumnoRepository.ObtenerPorId(prestamo.IdAlumno);
                        throw new Exception($"El alumno {alumno.NombreCompleto} tiene otros préstamos con más de {maxDiasAtraso} días de atraso. Debe devolverlos antes de renovar.");
                    }
                }
            }

            // Verificar que el ejemplar siga existiendo y no esté en mal estado
            if (prestamo.IdEjemplar != Guid.Empty)
            {
                var ejemplar = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);
                if (ejemplar == null)
                    throw new Exception("El ejemplar asociado ya no existe en el sistema");

                if (ejemplar.Estado == DomainModel.Enums.EstadoMaterial.NoDisponible)
                    throw new Exception("No se puede renovar un préstamo de un ejemplar reportado como no disponible");

                if (ejemplar.Estado == DomainModel.Enums.EstadoMaterial.EnReparacion)
                    throw new Exception("No se puede renovar un préstamo de un ejemplar en reparación");
            }

            // Calcular nueva fecha de devolución (desde HOY, no desde fecha original)
            DateTime nuevaFechaDevolucion = DateTime.Now.Date.AddDays(diasExtension);

            // Ejecutar la renovación (transacción atómica en el repository)
            _prestamoRepository.RenovarPrestamo(idPrestamo, nuevaFechaDevolucion, idUsuario, observaciones);

            // Si el préstamo estaba "Atrasado", actualizarlo a "Activo" ahora que se renovó
            if (prestamo.Estado == "Atrasado")
            {
                _prestamoRepository.ActualizarEstado(idPrestamo, "Activo");
            }
        }

        /// <summary>
        /// Obtiene el historial de renovaciones de un préstamo
        /// </summary>
        public List<RenovacionPrestamo> ObtenerHistorialRenovaciones(Guid idPrestamo)
        {
            return _prestamoRepository.ObtenerRenovacionesPorPrestamo(idPrestamo);
        }
    }
}
