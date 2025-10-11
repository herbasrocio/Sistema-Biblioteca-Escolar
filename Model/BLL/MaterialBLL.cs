using System;
using System.Collections.Generic;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    public class MaterialBLL
    {
        private readonly IMaterialRepository _materialRepository;

        // Constructor con inyección de dependencias
        public MaterialBLL(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        }

        // Constructor sin parámetros (crea la dependencia internamente)
        public MaterialBLL() : this(new MaterialRepository()) { }

        public List<Material> ObtenerTodosMateriales()
        {
            return _materialRepository.GetAll();
        }

        public Material ObtenerMaterialPorId(Guid idMaterial)
        {
            return _materialRepository.ObtenerPorId(idMaterial);
        }

        public List<Material> BuscarMateriales(string titulo, string autor, string tipo)
        {
            return _materialRepository.BuscarPorFiltros(titulo, autor, tipo);
        }

        public void GuardarMaterial(Material material)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(material.Titulo))
                throw new Exception("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(material.Autor))
                throw new Exception("El autor es obligatorio");

            if (string.IsNullOrWhiteSpace(material.Tipo))
                throw new Exception("El tipo es obligatorio");

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

        public void ActualizarMaterial(Material material)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(material.Titulo))
                throw new Exception("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(material.Autor))
                throw new Exception("El autor es obligatorio");

            if (string.IsNullOrWhiteSpace(material.Tipo))
                throw new Exception("El tipo es obligatorio");

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

        public void EliminarMaterial(Material material)
        {
            // Validar que no tenga préstamos activos
            // TODO: Implementar validación con préstamos cuando exista esa funcionalidad

            _materialRepository.Delete(material);
        }
    }
}
