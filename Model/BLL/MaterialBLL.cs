using System;
using System.Collections.Generic;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using DomainModel.Exceptions;

namespace BLL
{
    /// <summary>
    /// Proporciona la lógica de negocio para la gestión de materiales bibliográficos.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa las reglas de negocio relacionadas con materiales del catálogo,
    /// incluyendo validaciones, operaciones CRUD y búsquedas. Actúa como intermediario
    /// entre la capa de presentación y la capa de acceso a datos.
    /// </remarks>
    public class MaterialBLL
    {
        private readonly IMaterialRepository _materialRepository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MaterialBLL"/> con inyección de dependencias.
        /// </summary>
        /// <param name="materialRepository">Repositorio para acceso a datos de materiales.</param>
        /// <exception cref="ArgumentNullException">Se lanza si <paramref name="materialRepository"/> es <c>null</c>.</exception>
        public MaterialBLL(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MaterialBLL"/> sin inyección de dependencias.
        /// </summary>
        /// <remarks>
        /// Este constructor crea internamente una instancia de <see cref="MaterialRepository"/>.
        /// Se recomienda usar el constructor con inyección de dependencias para facilitar las pruebas.
        /// </remarks>
        public MaterialBLL() : this(new MaterialRepository()) { }

        /// <summary>
        /// Obtiene todos los materiales activos del catálogo.
        /// </summary>
        /// <returns>Lista de todos los materiales registrados y activos en el sistema.</returns>
        /// <remarks>
        /// Solo devuelve materiales con estado Activo=true. Las cantidades de ejemplares
        /// se calculan dinámicamente basándose en los ejemplares asociados.
        /// </remarks>
        public List<Material> ObtenerTodosMateriales()
        {
            return _materialRepository.GetAll();
        }

        /// <summary>
        /// Obtiene un material específico por su identificador único.
        /// </summary>
        /// <param name="idMaterial">Identificador único del material a buscar.</param>
        /// <returns>
        /// El objeto <see cref="Material"/> si se encuentra; de lo contrario, <c>null</c>.
        /// </returns>
        public Material ObtenerMaterialPorId(Guid idMaterial)
        {
            return _materialRepository.ObtenerPorId(idMaterial);
        }

        /// <summary>
        /// Busca materiales aplicando filtros opcionales de título, autor y tipo.
        /// </summary>
        /// <param name="titulo">Título o parte del título a buscar. Puede ser <c>null</c> o vacío para omitir este filtro.</param>
        /// <param name="autor">Autor o parte del nombre del autor a buscar. Puede ser <c>null</c> o vacío para omitir este filtro.</param>
        /// <param name="tipo">Tipo de material (Libro, Revista, Manual). Puede ser <c>null</c>, vacío o "Todos" para omitir este filtro.</param>
        /// <returns>Lista de materiales que coinciden con los criterios de búsqueda.</returns>
        /// <remarks>
        /// La búsqueda es insensible a mayúsculas/minúsculas y permite coincidencias parciales.
        /// Los filtros son opcionales y se combinan con operador AND.
        /// </remarks>
        public List<Material> BuscarMateriales(string titulo, string autor, string tipo)
        {
            return _materialRepository.BuscarPorFiltros(titulo, autor, tipo);
        }

        /// <summary>
        /// Guarda un nuevo material en el catálogo después de validar las reglas de negocio.
        /// </summary>
        /// <param name="material">Material a guardar en el sistema.</param>
        /// <exception cref="Exception">
        /// Se lanza si no se cumplen las validaciones:
        /// <list type="bullet">
        /// <item><description>El título es obligatorio.</description></item>
        /// <item><description>El autor es obligatorio.</description></item>
        /// <item><description>El género es obligatorio.</description></item>
        /// <item><description>La cantidad total no puede ser negativa.</description></item>
        /// <item><description>La cantidad disponible no puede ser negativa.</description></item>
        /// <item><description>La cantidad disponible no puede exceder la cantidad total.</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// El tipo de material se valida automáticamente por ser un enum.
        /// </remarks>
        public void GuardarMaterial(Material material)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(material.Titulo))
                throw new Exception("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(material.Autor))
                throw new Exception("El autor es obligatorio");

            // El Tipo es un enum, siempre tendrá un valor válido por defecto
            // No es necesario validarlo

            if (string.IsNullOrWhiteSpace(material.Genero))
                throw new Exception("El género es obligatorio");

            if (material.CantidadTotal < 0)
                throw new Exception("La cantidad total no puede ser negativa");

            if (material.CantidadDisponible < 0)
                throw new Exception("La cantidad disponible no puede ser negativa");

            if (material.CantidadDisponible > material.CantidadTotal)
                throw new Exception("La cantidad disponible no puede ser mayor a la cantidad total");

            _materialRepository.Add(material);
        }

        /// <summary>
        /// Actualiza un material existente en el catálogo después de validar las reglas de negocio.
        /// </summary>
        /// <param name="material">Material con los datos actualizados.</param>
        /// <exception cref="Exception">
        /// Se lanza si no se cumplen las validaciones:
        /// <list type="bullet">
        /// <item><description>El título es obligatorio.</description></item>
        /// <item><description>El autor es obligatorio.</description></item>
        /// <item><description>El género es obligatorio.</description></item>
        /// <item><description>La cantidad total no puede ser negativa.</description></item>
        /// <item><description>La cantidad disponible no puede ser negativa.</description></item>
        /// <item><description>La cantidad disponible no puede exceder la cantidad total.</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// El tipo de material se valida automáticamente por ser un enum.
        /// </remarks>
        public void ActualizarMaterial(Material material)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(material.Titulo))
                throw new Exception("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(material.Autor))
                throw new Exception("El autor es obligatorio");

            // El Tipo es un enum, siempre tendrá un valor válido por defecto
            // No es necesario validarlo

            if (string.IsNullOrWhiteSpace(material.Genero))
                throw new Exception("El género es obligatorio");

            if (material.CantidadTotal < 0)
                throw new Exception("La cantidad total no puede ser negativa");

            if (material.CantidadDisponible < 0)
                throw new Exception("La cantidad disponible no puede ser negativa");

            if (material.CantidadDisponible > material.CantidadTotal)
                throw new Exception("La cantidad disponible no puede ser mayor a la cantidad total");

            _materialRepository.Update(material);
        }

        /// <summary>
        /// Elimina lógicamente un material del catálogo si no tiene préstamos activos asociados.
        /// </summary>
        /// <param name="material">Material a eliminar del sistema.</param>
        /// <exception cref="ValidacionException">
        /// Se lanza si el material tiene préstamos activos. No se permite eliminar materiales
        /// que están actualmente prestados.
        /// </exception>
        /// <remarks>
        /// La eliminación es lógica (soft delete), marcando el material como Activo=false
        /// en lugar de eliminarlo físicamente de la base de datos.
        /// </remarks>
        public void EliminarMaterial(Material material)
        {
            // Validar que no tenga préstamos activos
            var prestamoBLL = new PrestamoBLL();
            var prestamosMaterial = prestamoBLL.ObtenerPorMaterial(material.IdMaterial);
            var prestamosActivos = prestamosMaterial.FindAll(p => p.Estado == "Activo");

            if (prestamosActivos.Count > 0)
            {
                throw new ValidacionException($"No se puede eliminar el material porque tiene {prestamosActivos.Count} préstamo(s) activo(s)");
            }

            _materialRepository.Delete(material);
        }
    }
}
