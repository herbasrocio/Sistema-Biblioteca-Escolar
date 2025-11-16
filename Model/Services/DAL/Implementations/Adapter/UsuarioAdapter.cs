using DAL.Contracts;
using Services.DomainModel.Security.Composite;
using Services.Services;
using DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Adapter
{


    public sealed class UsuarioAdapter : IAdapter<Usuario>
    {
        #region Singleton
        private readonly static UsuarioAdapter _instance = new UsuarioAdapter();

        public static UsuarioAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private UsuarioAdapter()
        {
            //Implement here the initialization code
        }
        #endregion
        public Usuario Adapt(object[] values)
        {
            //Hidratar el objeto usuario -> Nivel 1
            // Orden: IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, FechaUltimoAcceso, DVH
            Usuario usuario = new Usuario()
            {
                IdUsuario = Guid.Parse(values[0].ToString()),
                Nombre = values[1].ToString(),
                Email = values.Length > 2 && values[2] != DBNull.Value ? values[2].ToString() : null,
                Clave = values[3].ToString(),
                Activo = values.Length > 4 && values[4] != DBNull.Value ? Convert.ToBoolean(values[4]) : true,
                IdiomaPreferido = values.Length > 5 && values[5] != DBNull.Value ? values[5].ToString() : "es-AR",
                FechaUltimoAcceso = values.Length > 6 && values[6] != DBNull.Value ? Convert.ToDateTime(values[6]) : (DateTime?)null,
                DVH = values.Length > 7 && values[7] != DBNull.Value ? values[7].ToString() : null
            };

            //Nivel 2 de hidratación...
            try
            {
                List<Component> components = new List<Component>();
                var veremos = UsuarioFamiliaRepository.Current.GetChildren(usuario);

                foreach (var item in veremos)
                {
                    Familia familia = LoginService.SelectOneFamilia(item.idFamilia);
                    usuario.Permisos.Add(familia);
                }
            }
            catch (Exception)
            {
                // Re-lanzar la excepción para que sea manejada por el llamador
                throw;
            }

            return usuario;
        }
    }
}
