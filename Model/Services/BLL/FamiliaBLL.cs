using System;
using System.Collections.Generic;
using System.Linq;
using Services.DomainModel.Security.Composite;
using Services.DomainModel.Exceptions;
using Services.Services;

namespace Services.BLL
{
    public static class FamiliaBLL
    {
        /// <summary>
        /// Crea un nuevo rol (Familia) con el nombre especificado
        /// Los roles deben tener nombres que comiencen con "ROL_"
        /// </summary>
        /// <param name="nombreRol">Nombre del rol sin el prefijo "ROL_"</param>
        /// <returns>La familia (rol) creada</returns>
        public static Familia CrearRol(string nombreRol)
        {
            try
            {
                // Validar nombre
                if (string.IsNullOrWhiteSpace(nombreRol))
                {
                    throw new ValidacionException("El nombre del rol no puede estar vacío");
                }

                // Validar longitud mínima
                if (nombreRol.Trim().Length < 3)
                {
                    throw new ValidacionException("El nombre del rol debe tener al menos 3 caracteres");
                }

                // Asegurarse de que el nombre tenga el prefijo "ROL_"
                string nombreCompleto = nombreRol.StartsWith("ROL_")
                    ? nombreRol
                    : $"ROL_{nombreRol.Trim()}";

                // Verificar que no exista un rol con el mismo nombre
                var roles = DAL.Implementations.FamiliaRepository.Current.SelectAll();
                if (roles.Any(r => r.Nombre.Equals(nombreCompleto, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidacionException($"Ya existe un rol con el nombre '{nombreCompleto}'");
                }

                // Crear la nueva familia (rol)
                var nuevoRol = new Familia
                {
                    IdComponent = Guid.NewGuid(),
                    Nombre = nombreCompleto
                };

                // Insertar en la base de datos
                DAL.Implementations.FamiliaRepository.Current.Add(nuevoRol);

                return nuevoRol;
            }
            catch (ValidacionException)
            {
                // Re-lanzar excepciones de validación sin envolver
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al crear el rol", ex);
            }
        }

        /// <summary>
        /// Elimina un rol (Familia) del sistema
        /// Valida que no tenga usuarios asignados antes de eliminar
        /// </summary>
        /// <param name="idFamilia">ID del rol a eliminar</param>
        public static void EliminarRol(Guid idFamilia)
        {
            try
            {
                // Verificar que la familia existe y es un rol
                var familia = DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                {
                    throw new ValidacionException("El rol seleccionado no existe");
                }

                if (!familia.EsRol)
                {
                    throw new ValidacionException("La familia seleccionada no es un rol válido");
                }

                // Verificar que no tenga usuarios asignados
                var usuariosConRol = DAL.Implementations.UsuarioFamiliaRepository.Current
                    .SelectAll()
                    .Where(uf => uf.idFamilia == idFamilia);

                if (usuariosConRol.Any())
                {
                    throw new ValidacionException($"No se puede eliminar el rol '{familia.Nombre}' porque tiene usuarios asignados");
                }

                // Eliminar todas las relaciones FamiliaPatente
                var familiaPatentes = DAL.Implementations.FamiliaPatenteRepository.Current
                    .GetChildrenRelations(familia);

                foreach (var fp in familiaPatentes)
                {
                    var relacion = new DomainModel.Security.FamiliaPatente
                    {
                        idFamilia = fp.idFamilia,
                        idPatente = fp.idPatente
                    };
                    DAL.Implementations.FamiliaPatenteRepository.Current.DeleteRelacion(relacion);
                }

                // Eliminar el rol
                DAL.Implementations.FamiliaRepository.Current.Delete(idFamilia);
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al eliminar el rol", ex);
            }
        }

        /// <summary>
        /// Actualiza las patentes de un rol (Familia)
        /// Elimina todas las patentes actuales y asigna las nuevas
        /// </summary>
        public static void ActualizarPatentesDeRol(Guid idFamilia, List<Patente> patentes)
        {
            try
            {
                // Verificar que la familia existe y es un rol
                var familia = DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                {
                    throw new ValidacionException("La familia seleccionada no existe");
                }

                if (!familia.EsRol)
                {
                    throw new ValidacionException("La familia seleccionada no es un rol válido");
                }

                // Obtener patentes actuales directas de la familia
                var patentesActuales = ObtenerPatentesDirectasDeFamilia(idFamilia);

                // Eliminar patentes que ya no están en la lista
                foreach (var patenteActual in patentesActuales)
                {
                    if (!patentes.Any(p => p.IdComponent == patenteActual.IdComponent))
                    {
                        QuitarPatenteDeFamilia(idFamilia, patenteActual.IdComponent);
                    }
                }

                // Agregar patentes nuevas
                foreach (var patente in patentes)
                {
                    if (!patentesActuales.Any(p => p.IdComponent == patente.IdComponent))
                    {
                        AsignarPatenteAFamilia(idFamilia, patente.IdComponent);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al actualizar patentes del rol", ex);
            }
        }

        /// <summary>
        /// Obtiene las patentes asignadas directamente a una familia (no recursivo)
        /// </summary>
        public static IEnumerable<Patente> ObtenerPatentesDirectasDeFamilia(Guid idFamilia)
        {
            try
            {
                var familia = DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                {
                    throw new ValidacionException("La familia no existe");
                }

                var familiaPatentes = DAL.Implementations.FamiliaPatenteRepository.Current
                    .GetChildrenRelations(familia);

                List<Patente> patentes = new List<Patente>();
                foreach (var fp in familiaPatentes)
                {
                    var patente = DAL.Implementations.PatenteRepository.Current.SelectOne(fp.idPatente);
                    if (patente != null)
                    {
                        patentes.Add(patente);
                    }
                }

                return patentes;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener patentes de la familia", ex);
            }
        }

        /// <summary>
        /// Asigna una patente a una familia
        /// </summary>
        public static void AsignarPatenteAFamilia(Guid idFamilia, Guid idPatente)
        {
            try
            {
                var familiaPatente = new DomainModel.Security.FamiliaPatente
                {
                    idFamilia = idFamilia,
                    idPatente = idPatente
                };

                DAL.Implementations.FamiliaPatenteRepository.Current.Insert(familiaPatente);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al asignar patente a la familia", ex);
            }
        }

        /// <summary>
        /// Quita una patente de una familia
        /// </summary>
        public static void QuitarPatenteDeFamilia(Guid idFamilia, Guid idPatente)
        {
            try
            {
                var familiaPatente = new DomainModel.Security.FamiliaPatente
                {
                    idFamilia = idFamilia,
                    idPatente = idPatente
                };

                DAL.Implementations.FamiliaPatenteRepository.Current.DeleteRelacion(familiaPatente);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar patente de la familia", ex);
            }
        }
    }
}
