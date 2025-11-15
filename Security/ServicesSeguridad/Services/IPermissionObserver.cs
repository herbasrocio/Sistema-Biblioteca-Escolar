using System;
using ServicesSecurity.DomainModel.Security.Composite;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Interfaz Observer para el patrón Observer de actualización de permisos.
    /// Los formularios que implementen esta interfaz serán notificados cuando
    /// cambien los permisos del usuario actual.
    /// </summary>
    public interface IPermissionObserver
    {
        /// <summary>
        /// Método llamado cuando los permisos del usuario han cambiado.
        /// Los formularios deben actualizar su interfaz según los nuevos permisos.
        /// </summary>
        /// <param name="usuario">Usuario con los permisos actualizados</param>
        void OnPermissionsChanged(Usuario usuario);
    }
}
