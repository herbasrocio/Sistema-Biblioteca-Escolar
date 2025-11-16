using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DomainModel.Exceptions
{
    public class ContraseñaInvalidaException : AutenticacionException
    {
        public ContraseñaInvalidaException()
            : base("La contraseña ingresada es incorrecta")
        {
        }

        public ContraseñaInvalidaException(string message) : base(message)
        {
        }
    }
}
