using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Services;

namespace Services.DomainModel.Security.Composite
{
    public class Usuario
    {
        public Guid IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Clave { get; set; } // Alias para compatibilidad con BD

        public bool Activo { get; set; }

        public string IdiomaPreferido { get; set; } // Idioma preferido del usuario (ej: "es-AR", "en-GB")

        public DateTime? FechaUltimoAcceso { get; set; } // Fecha del último acceso del usuario

        public string DVH { get; set; } // Dígito Verificador Horizontal

        public string HashDH
        {
            get
            {
                return CryptographyService.HashPassword(Nombre + Password);
            }
        }

        public string HashPassword
        {
            get
            {
                return CryptographyService.HashPassword(this.Password);
            }
        }

        public List<Component> Permisos { get; set; }

        public Usuario()
        {
            Permisos = new List<Component>();
        }

        /// <summary>
        /// Obtiene la Familia que representa el Rol del usuario
        /// Busca en los permisos la primera Familia con nombre "ROL_*"
        /// </summary>
        /// <returns>Familia de rol o null si no tiene rol asignado</returns>
        public Familia ObtenerFamiliaRol()
        {
            foreach (var permiso in Permisos)
            {
                if (permiso is Familia familia && familia.EsRol)
                {
                    return familia;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene el nombre del rol del usuario (sin prefijo "ROL_")
        /// Ejemplo: retorna "Administrador", "Veterinario", etc.
        /// </summary>
        /// <returns>Nombre del rol o null si no tiene rol</returns>
        public string ObtenerNombreRol()
        {
            var familiaRol = ObtenerFamiliaRol();
            return familiaRol?.ObtenerNombreRol();
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico
        /// </summary>
        /// <param name="nombreRol">Nombre del rol sin prefijo (ej: "Administrador")</param>
        /// <returns>True si tiene ese rol</returns>
        public bool TieneRol(string nombreRol)
        {
            var rolActual = ObtenerNombreRol();
            return rolActual != null && rolActual.Equals(nombreRol, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si el usuario tiene un permiso específico
        /// Verifica en el árbol de permisos asignados (Familias y Patentes)
        /// </summary>
        /// <param name="nombrePatente">Nombre del permiso (FormName o MenuItemName)</param>
        /// <returns>True si tiene el permiso asignado</returns>
        public bool TienePermiso(string nombrePatente)
        {
            // Buscar el permiso en el árbol de permisos del usuario
            if (Permisos == null)
                return false;

            foreach (var componente in Permisos)
            {
                if (TienePermisoRecursivo(componente, nombrePatente))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Método recursivo para buscar un permiso en el árbol de componentes
        /// </summary>
        private bool TienePermisoRecursivo(Component componente, string nombrePatente)
        {
            if (componente == null)
                return false;

            // Si es una Patente, verificar coincidencia por FormName o MenuItemName
            if (componente is Patente patente)
            {
                return (patente.FormName != null && patente.FormName.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase)) ||
                       (patente.MenuItemName != null && patente.MenuItemName.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase));
            }

            // Si es una Familia, buscar recursivamente en sus hijos
            if (componente is Familia familia)
            {
                foreach (var hijo in familia.GetChildrens())
                {
                    if (hijo != null && TienePermisoRecursivo(hijo, nombrePatente))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retornar las patentes únicas de acuerdo a mi modelo
        /// (Para el armado del menú)
        /// </summary>
        /// <returns></returns>
        public List<Patente> GetPatentesAll()
        {
            List<Patente> patentesDistinct = new List<Patente>();

            RecorrerComposite(patentesDistinct, Permisos, "-");

            return patentesDistinct;
        }

        private static void RecorrerComposite(List<Patente> patentes, List<Component> components, string tab)
        {
            foreach (var item in components)
            {
                if (item.ChildrenCount() == 0)
                {
                    //Estoy ante un elemento de tipo Patente
                    Patente patente1 = item as Patente;
                    Console.WriteLine($"{tab} Patente: {patente1.FormName}");

                    if (!patentes.Exists(o => o.FormName == patente1.FormName))
                        patentes.Add(patente1);

                    //bool existe = false;

                    //foreach (var item2 in patentes)
                    //{
                    //    if(item2.FormName == patente1.FormName)
                    //    {
                    //        existe = true;
                    //        break;
                    //    }
                    //}

                    //if(!existe)
                    //    patentes.Add(patente1);
                }
                else
                {
                    Familia familia = item as Familia;
                    Console.WriteLine($"{tab} Familia: {familia.Nombre}");
                    RecorrerComposite(patentes, familia.GetChildrens(), tab + "-");
                }
            }
        }

    }
}
