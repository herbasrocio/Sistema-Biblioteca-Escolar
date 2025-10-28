using System;
using System.Data.SqlClient;
using System.Transactions;
using DAL.Contracts;

namespace DAL.Implementations
{
    /// <summary>
    /// Implementación del patrón Unit of Work usando TransactionScope
    /// Gestiona una transacción única compartida entre múltiples repositorios
    /// Este enfoque usa TransactionScope en lugar de SqlTransaction para evitar
    /// tener que refactorizar todos los repositorios existentes
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transactionScope;
        private bool _disposed = false;

        // Repositorios lazy-loaded (usan sus constructores por defecto)
        private IPrestamoRepository _prestamos;
        private IEjemplarRepository _ejemplares;
        private IDevolucionRepository _devoluciones;
        private IMaterialRepository _materiales;
        private IAlumnoRepository _alumnos;

        public UnitOfWork()
        {
            // No se crea la transacción aquí, se espera a BeginTransaction()
        }

        public IPrestamoRepository Prestamos
        {
            get
            {
                if (_prestamos == null)
                {
                    _prestamos = new PrestamoRepository();
                }
                return _prestamos;
            }
        }

        public IEjemplarRepository Ejemplares
        {
            get
            {
                if (_ejemplares == null)
                {
                    _ejemplares = new EjemplarRepository();
                }
                return _ejemplares;
            }
        }

        public IDevolucionRepository Devoluciones
        {
            get
            {
                if (_devoluciones == null)
                {
                    _devoluciones = new DevolucionRepository();
                }
                return _devoluciones;
            }
        }

        public IMaterialRepository Materiales
        {
            get
            {
                if (_materiales == null)
                {
                    _materiales = new MaterialRepository();
                }
                return _materiales;
            }
        }

        public IAlumnoRepository Alumnos
        {
            get
            {
                if (_alumnos == null)
                {
                    _alumnos = new AlumnoRepository();
                }
                return _alumnos;
            }
        }

        public bool HasActiveTransaction => _transactionScope != null;

        public void BeginTransaction()
        {
            if (_transactionScope != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa. " +
                    "Debe confirmar o revertir la transacción actual antes de iniciar una nueva.");
            }

            // Crear TransactionScope con opciones específicas
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(2)
            };

            _transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                options,
                TransactionScopeAsyncFlowOption.Enabled
            );
        }

        public void Commit()
        {
            if (_transactionScope == null)
            {
                throw new InvalidOperationException("No hay una transacción activa para confirmar. " +
                    "Debe llamar a BeginTransaction() primero.");
            }

            try
            {
                // Complete() indica que la transacción fue exitosa
                _transactionScope.Complete();
            }
            catch
            {
                // Si el commit falla, TransactionScope hace rollback automáticamente
                throw;
            }
            finally
            {
                // Dispose del TransactionScope (esto finaliza la transacción)
                if (_transactionScope != null)
                {
                    _transactionScope.Dispose();
                    _transactionScope = null;
                }
            }
        }

        public void Rollback()
        {
            if (_transactionScope == null)
            {
                // No lanzar excepción si no hay transacción activa al hacer rollback
                // Esto permite que Rollback se llame en bloques catch de forma segura
                return;
            }

            // Con TransactionScope, simplemente dispose sin llamar Complete()
            // Esto causa un rollback automático
            try
            {
                if (_transactionScope != null)
                {
                    _transactionScope.Dispose();
                    _transactionScope = null;
                }
            }
            catch
            {
                // Ignorar errores en rollback
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Si hay una transacción activa al momento de dispose, hacer rollback
                    // (dispose sin Complete() = rollback automático)
                    if (_transactionScope != null)
                    {
                        _transactionScope.Dispose();
                        _transactionScope = null;
                    }

                    // Los repositorios no necesitan dispose explícito
                    // ya que no mantienen recursos propios
                    _prestamos = null;
                    _ejemplares = null;
                    _devoluciones = null;
                    _materiales = null;
                    _alumnos = null;
                }

                _disposed = true;
            }
        }
    }
}
