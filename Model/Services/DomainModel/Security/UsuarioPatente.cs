using System;

namespace Services.DomainModel.Security
{
    /// <summary>
    /// Clase intermedia para la relación muchos-a-muchos entre Usuario y Patente
    /// </summary>
    public class UsuarioPatente
    {
        public Guid idUsuario { get; set; }
        public Guid idPatente { get; set; }
    }
}
