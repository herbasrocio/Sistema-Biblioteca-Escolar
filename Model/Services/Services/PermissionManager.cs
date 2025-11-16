using System;
using System.Collections.Generic;
using System.Linq;
using Services.BLL;
using Services.DomainModel.Security.Composite;

namespace Services.Services
{
    /// <summary>
    /// Subject del patrón Observer para gestión de permisos en tiempo real.
    /// Implementa Singleton para asegurar una única instancia centralizada.
    /// Notifica a todos los observadores cuando los permisos de un usuario cambian.
    /// </summary>
    public class PermissionManager
    {
        #region Singleton

        private static readonly PermissionManager _instance = new PermissionManager();

        public static PermissionManager Instance
        {
            get { return _instance; }
        }

        private PermissionManager()
        {
            _observers = new List<IPermissionObserver>();
        }

        #endregion

        #region Observer Pattern

        private List<IPermissionObserver> _observers;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Registra un observador que será notificado cuando cambien los permisos
        /// </summary>
        public void Attach(IPermissionObserver observer)
        {
            lock (_lockObject)
            {
                if (observer != null && !_observers.Contains(observer))
                {
                    _observers.Add(observer);
                }
            }
        }

        /// <summary>
        /// Desregistra un observador
        /// </summary>
        public void Detach(IPermissionObserver observer)
        {
            lock (_lockObject)
            {
                if (observer != null && _observers.Contains(observer))
                {
                    _observers.Remove(observer);
                }
            }
        }

        /// <summary>
        /// Notifica a todos los observadores que los permisos del usuario han cambiado.
        /// Recarga los permisos del usuario desde la base de datos y notifica.
        /// </summary>
        /// <param name="idUsuario">ID del usuario cuyos permisos cambiaron</param>
        public void NotifyPermissionsChanged(Guid idUsuario)
        {
            try
            {
                // Recargar permisos actualizados del usuario desde la base de datos
                Usuario usuarioActualizado = UsuarioBLL.ObtenerUsuarioPorId(idUsuario);

                if (usuarioActualizado == null)
                    return;

                // Notificar a todos los observadores
                lock (_lockObject)
                {
                    // Crear una copia de la lista para evitar problemas si un observador se desregistra durante la notificación
                    var observersCopy = _observers.ToList();

                    foreach (var observer in observersCopy)
                    {
                        try
                        {
                            observer.OnPermissionsChanged(usuarioActualizado);
                        }
                        catch (Exception ex)
                        {
                            // Log del error pero continuar notificando a otros observadores
                            System.Diagnostics.Debug.WriteLine($"Error al notificar observador: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al recargar permisos: {ex.Message}");
            }
        }

        /// <summary>
        /// Notifica cambios de permisos a todos los usuarios (usado cuando se modifica una familia/rol)
        /// </summary>
        public void NotifyAllUsersPermissionsChanged()
        {
            lock (_lockObject)
            {
                var observersCopy = _observers.ToList();

                foreach (var observer in observersCopy)
                {
                    try
                    {
                        // En este caso, pasamos null para indicar que el observador debe recargar el usuario actual
                        observer.OnPermissionsChanged(null);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al notificar observador: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Limpia todos los observadores (útil en logout)
        /// </summary>
        public void ClearObservers()
        {
            lock (_lockObject)
            {
                _observers.Clear();
            }
        }

        #endregion
    }
}
