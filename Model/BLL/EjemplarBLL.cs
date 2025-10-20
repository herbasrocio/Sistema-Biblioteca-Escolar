using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using DomainModel.Enums;

namespace BLL
{
    public class EjemplarBLL
    {
        private readonly IEjemplarRepository _ejemplarRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IHistorialEstadoEjemplarRepository _historialRepository;

        // Constructor con inyección de dependencias
        public EjemplarBLL(IEjemplarRepository ejemplarRepository, IMaterialRepository materialRepository, IHistorialEstadoEjemplarRepository historialRepository)
        {
            _ejemplarRepository = ejemplarRepository ?? throw new ArgumentNullException(nameof(ejemplarRepository));
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
            _historialRepository = historialRepository ?? throw new ArgumentNullException(nameof(historialRepository));
        }

        // Constructor sin parámetros (crea las dependencias internamente)
        public EjemplarBLL() : this(new EjemplarRepository(), new MaterialRepository(), new HistorialEstadoEjemplarRepository()) { }

        /// <summary>
        /// Obtiene todos los ejemplares activos
        /// </summary>
        public List<Ejemplar> ObtenerTodosEjemplares()
        {
            return _ejemplarRepository.GetAll();
        }

        /// <summary>
        /// Obtiene todos los ejemplares de un material específico
        /// </summary>
        public List<Ejemplar> ObtenerEjemplaresPorMaterial(Guid idMaterial)
        {
            return _ejemplarRepository.ObtenerPorMaterial(idMaterial);
        }

        /// <summary>
        /// Obtiene un ejemplar por su ID
        /// </summary>
        public Ejemplar ObtenerEjemplarPorId(Guid idEjemplar)
        {
            return _ejemplarRepository.ObtenerPorId(idEjemplar);
        }

        /// <summary>
        /// Obtiene ejemplares por estado
        /// </summary>
        public List<Ejemplar> ObtenerEjemplaresPorEstado(EstadoMaterial estado)
        {
            return _ejemplarRepository.ObtenerPorEstado(estado);
        }

        /// <summary>
        /// Busca un ejemplar por código de barras
        /// </summary>
        public Ejemplar BuscarPorCodigoEjemplar(string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                throw new ArgumentException("El código de barras no puede estar vacío");

            return _ejemplarRepository.ObtenerPorCodigoEjemplar(codigoBarras);
        }

        /// <summary>
        /// Guarda un nuevo ejemplar y actualiza las cantidades del material
        /// </summary>
        public void GuardarEjemplar(Ejemplar ejemplar)
        {
            // Validaciones
            if (ejemplar.IdMaterial == Guid.Empty)
                throw new Exception("El ejemplar debe estar asociado a un material");

            Material material = _materialRepository.ObtenerPorId(ejemplar.IdMaterial);
            if (material == null)
                throw new Exception("El material asociado no existe");

            if (ejemplar.NumeroEjemplar <= 0)
                throw new Exception("El número de ejemplar debe ser mayor a cero");

            // Verificar que el número de ejemplar no esté duplicado para este material
            var ejemplaresExistentes = _ejemplarRepository.ObtenerPorMaterial(ejemplar.IdMaterial);
            if (ejemplaresExistentes.Any(e => e.NumeroEjemplar == ejemplar.NumeroEjemplar && e.IdEjemplar != ejemplar.IdEjemplar))
                throw new Exception($"Ya existe un ejemplar con el número {ejemplar.NumeroEjemplar} para este material");

            // Si tiene código de barras, verificar que sea único
            if (!string.IsNullOrWhiteSpace(ejemplar.CodigoEjemplar))
            {
                var ejemplarConCodigo = _ejemplarRepository.ObtenerPorCodigoEjemplar(ejemplar.CodigoEjemplar);
                if (ejemplarConCodigo != null && ejemplarConCodigo.IdEjemplar != ejemplar.IdEjemplar)
                    throw new Exception("El código de barras ya está en uso por otro ejemplar");
            }

            _ejemplarRepository.Add(ejemplar);

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(ejemplar.IdMaterial);
        }

        /// <summary>
        /// Actualiza un ejemplar existente
        /// </summary>
        public void ActualizarEjemplar(Ejemplar ejemplar)
        {
            // Validaciones
            if (ejemplar.IdEjemplar == Guid.Empty)
                throw new Exception("El ID del ejemplar es inválido");

            if (ejemplar.NumeroEjemplar <= 0)
                throw new Exception("El número de ejemplar debe ser mayor a cero");

            // Si tiene código de barras, verificar que sea único
            if (!string.IsNullOrWhiteSpace(ejemplar.CodigoEjemplar))
            {
                var ejemplarConCodigo = _ejemplarRepository.ObtenerPorCodigoEjemplar(ejemplar.CodigoEjemplar);
                if (ejemplarConCodigo != null && ejemplarConCodigo.IdEjemplar != ejemplar.IdEjemplar)
                    throw new Exception("El código de barras ya está en uso por otro ejemplar");
            }

            _ejemplarRepository.Update(ejemplar);

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(ejemplar.IdMaterial);
        }

        /// <summary>
        /// Cambia el estado de un ejemplar y registra el cambio en el historial
        /// </summary>
        public void CambiarEstado(Guid idEjemplar, EstadoMaterial nuevoEstado, Guid? idUsuario = null, string motivo = null)
        {
            var ejemplar = _ejemplarRepository.ObtenerPorId(idEjemplar);
            if (ejemplar == null)
                throw new Exception("El ejemplar no existe");

            // Guardar estado anterior para el historial
            EstadoMaterial estadoAnterior = ejemplar.Estado;

            // Solo registrar si el estado realmente cambia
            if (estadoAnterior != nuevoEstado)
            {
                // Actualizar estado del ejemplar
                _ejemplarRepository.ActualizarEstado(idEjemplar, nuevoEstado);

                // Registrar cambio en historial
                var historial = new HistorialEstadoEjemplar
                {
                    IdEjemplar = idEjemplar,
                    EstadoAnterior = estadoAnterior,
                    EstadoNuevo = nuevoEstado,
                    IdUsuario = idUsuario,
                    Motivo = motivo,
                    TipoCambio = TipoCambioEstado.Manual
                };

                _historialRepository.RegistrarCambio(historial);
            }

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(ejemplar.IdMaterial);
        }

        /// <summary>
        /// Elimina un ejemplar (borrado lógico)
        /// </summary>
        public void EliminarEjemplar(Ejemplar ejemplar)
        {
            // Validar que no esté prestado
            if (ejemplar.Estado == EstadoMaterial.Prestado)
                throw new Exception("No se puede eliminar un ejemplar que está prestado");

            Guid idMaterial = ejemplar.IdMaterial;
            _ejemplarRepository.Delete(ejemplar);

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(idMaterial);
        }

        /// <summary>
        /// Crea ejemplares automáticamente para un material nuevo
        /// </summary>
        public void CrearEjemplaresParaMaterial(Guid idMaterial, int cantidad)
        {
            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero");

            Material material = _materialRepository.ObtenerPorId(idMaterial);
            if (material == null)
                throw new Exception("El material no existe");

            // Obtener el número del último ejemplar existente
            var ejemplaresExistentes = _ejemplarRepository.ObtenerPorMaterial(idMaterial);
            int ultimoNumero = ejemplaresExistentes.Any() ? ejemplaresExistentes.Max(e => e.NumeroEjemplar) : 0;

            // Crear los nuevos ejemplares
            for (int i = 1; i <= cantidad; i++)
            {
                Ejemplar nuevoEjemplar = new Ejemplar
                {
                    IdMaterial = idMaterial,
                    NumeroEjemplar = ultimoNumero + i,
                    Estado = EstadoMaterial.Disponible
                };

                _ejemplarRepository.Add(nuevoEjemplar);
            }

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(idMaterial);
        }

        /// <summary>
        /// Recalcula y actualiza las cantidades total y disponible de un material
        /// basándose en sus ejemplares
        /// </summary>
        private void ActualizarCantidadesMaterial(Guid idMaterial)
        {
            Material material = _materialRepository.ObtenerPorId(idMaterial);
            if (material == null)
                return;

            var ejemplares = _ejemplarRepository.ObtenerPorMaterial(idMaterial);

            // Contar solo ejemplares activos
            int cantidadTotal = ejemplares.Count;
            int cantidadDisponible = ejemplares.Count(e => e.Estado == EstadoMaterial.Disponible);

            // Actualizar material
            material.CantidadTotal = cantidadTotal;
            material.CantidadDisponible = cantidadDisponible;

            _materialRepository.Update(material);
        }

        /// <summary>
        /// Obtiene estadísticas de ejemplares por material
        /// </summary>
        public Dictionary<EstadoMaterial, int> ObtenerEstadisticasPorMaterial(Guid idMaterial)
        {
            var ejemplares = _ejemplarRepository.ObtenerPorMaterial(idMaterial);

            return new Dictionary<EstadoMaterial, int>
            {
                { EstadoMaterial.Disponible, ejemplares.Count(e => e.Estado == EstadoMaterial.Disponible) },
                { EstadoMaterial.Prestado, ejemplares.Count(e => e.Estado == EstadoMaterial.Prestado) },
                { EstadoMaterial.EnReparacion, ejemplares.Count(e => e.Estado == EstadoMaterial.EnReparacion) },
                { EstadoMaterial.NoDisponible, ejemplares.Count(e => e.Estado == EstadoMaterial.NoDisponible) }
            };
        }

        /// <summary>
        /// Marca un ejemplar como prestado
        /// Usado por el sistema de préstamos
        /// </summary>
        public void PrestarEjemplar(Guid idEjemplar)
        {
            var ejemplar = _ejemplarRepository.ObtenerPorId(idEjemplar);
            if (ejemplar == null)
                throw new Exception("El ejemplar no existe");

            if (ejemplar.Estado == EstadoMaterial.Prestado)
                throw new Exception("El ejemplar ya está prestado");

            if (ejemplar.Estado != EstadoMaterial.Disponible)
                throw new Exception($"El ejemplar no está disponible. Estado actual: {ejemplar.Estado}");

            if (!ejemplar.Activo)
                throw new Exception("El ejemplar no está activo");

            // Cambiar estado a Prestado
            _ejemplarRepository.ActualizarEstado(idEjemplar, EstadoMaterial.Prestado);

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(ejemplar.IdMaterial);
        }

        /// <summary>
        /// Marca un ejemplar como prestado usando código de barras
        /// </summary>
        public void PrestarEjemplarPorCodigoEjemplar(string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                throw new ArgumentException("El código de barras no puede estar vacío");

            var ejemplar = _ejemplarRepository.ObtenerPorCodigoEjemplar(codigoBarras);
            if (ejemplar == null)
                throw new Exception("No se encontró un ejemplar con ese código de barras");

            PrestarEjemplar(ejemplar.IdEjemplar);
        }

        /// <summary>
        /// Marca un ejemplar como disponible (devuelto)
        /// Usado por el sistema de devoluciones
        /// </summary>
        public void DevolverEjemplar(Guid idEjemplar)
        {
            var ejemplar = _ejemplarRepository.ObtenerPorId(idEjemplar);
            if (ejemplar == null)
                throw new Exception("El ejemplar no existe");

            if (ejemplar.Estado != EstadoMaterial.Prestado)
                throw new Exception($"El ejemplar no está prestado. Estado actual: {ejemplar.Estado}");

            if (!ejemplar.Activo)
                throw new Exception("El ejemplar no está activo");

            // Cambiar estado a Disponible
            _ejemplarRepository.ActualizarEstado(idEjemplar, EstadoMaterial.Disponible);

            // Actualizar cantidades del material
            ActualizarCantidadesMaterial(ejemplar.IdMaterial);
        }

        /// <summary>
        /// Marca un ejemplar como disponible usando código de barras
        /// </summary>
        public void DevolverEjemplarPorCodigoEjemplar(string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                throw new ArgumentException("El código de barras no puede estar vacío");

            var ejemplar = _ejemplarRepository.ObtenerPorCodigoEjemplar(codigoBarras);
            if (ejemplar == null)
                throw new Exception("No se encontró un ejemplar con ese código de barras");

            DevolverEjemplar(ejemplar.IdEjemplar);
        }

        /// <summary>
        /// Obtiene el primer ejemplar disponible de un material
        /// Útil para el sistema de préstamos cuando no se especifica un ejemplar
        /// </summary>
        public Ejemplar ObtenerPrimerEjemplarDisponible(Guid idMaterial)
        {
            var ejemplares = _ejemplarRepository.ObtenerPorMaterial(idMaterial);
            return ejemplares.FirstOrDefault(e => e.Estado == EstadoMaterial.Disponible && e.Activo);
        }

        /// <summary>
        /// Verifica si un material tiene ejemplares disponibles
        /// </summary>
        public bool TieneEjemplaresDisponibles(Guid idMaterial)
        {
            return _ejemplarRepository.ContarDisponiblesPorMaterial(idMaterial) > 0;
        }

        /// <summary>
        /// Obtiene el historial completo de cambios de estado de un ejemplar
        /// </summary>
        public List<HistorialEstadoEjemplar> ObtenerHistorialEstados(Guid idEjemplar)
        {
            return _historialRepository.ObtenerHistorialPorEjemplar(idEjemplar);
        }

        /// <summary>
        /// Obtiene el último cambio de estado de un ejemplar
        /// </summary>
        public HistorialEstadoEjemplar ObtenerUltimoCambioEstado(Guid idEjemplar)
        {
            return _historialRepository.ObtenerUltimoCambio(idEjemplar);
        }
    }
}
