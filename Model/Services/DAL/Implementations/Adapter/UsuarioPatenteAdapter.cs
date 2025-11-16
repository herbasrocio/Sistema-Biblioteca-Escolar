using DAL.Contracts;
using Services.DomainModel.Security.Composite;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DomainModel.Security;

namespace DAL.Implementations.Adapter
{
    public sealed class UsuarioPatenteAdapter : IEntityAdapter<UsuarioPatente>
    {
        #region singleton
        private readonly static UsuarioPatenteAdapter _instance = new UsuarioPatenteAdapter();

        public static UsuarioPatenteAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private UsuarioPatenteAdapter()
        {
            //Implent here the initialization of your singleton
        }
        #endregion 
        public UsuarioPatente Adapt(object[] values)
        {
            try
            {
                return new UsuarioPatente() ///HIDRATA OBJETO UsuarioPatente con lo que le tiró la DAL interna 
                {
                    //idUsuarioPatente = Guid.Parse(values[0].ToString()),
                    idUsuario = Guid.Parse(values[0].ToString()),
                    idPatente = Guid.Parse(values[1].ToString()),
                };
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
