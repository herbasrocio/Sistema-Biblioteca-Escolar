using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementations
{
    /// <summary>
    /// Clase base para repositorios que soporta tanto modo independiente como Unit of Work
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;
        protected SqlConnection _externalConnection;
        protected SqlTransaction _externalTransaction;
        protected readonly bool _isUnitOfWorkMode;

        /// <summary>
        /// Constructor para modo independiente (sin Unit of Work)
        /// </summary>
        protected BaseRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["NegocioConString"].ConnectionString;
            _isUnitOfWorkMode = false;
        }

        /// <summary>
        /// Constructor para modo Unit of Work
        /// </summary>
        /// <param name="connection">Conexión compartida</param>
        /// <param name="transaction">Transacción compartida</param>
        protected BaseRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _externalConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _externalTransaction = transaction;
            _isUnitOfWorkMode = true;
        }

        /// <summary>
        /// Ejecuta una operación de comando (INSERT, UPDATE, DELETE)
        /// Gestiona automáticamente la conexión según el modo
        /// </summary>
        protected void ExecuteCommand(Action<SqlCommand, SqlConnection, SqlTransaction> commandAction)
        {
            if (_isUnitOfWorkMode)
            {
                // Modo Unit of Work: usar conexión y transacción externa
                if (_externalConnection == null)
                    throw new InvalidOperationException("Conexión externa es null en modo Unit of Work");

                if (_externalConnection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Conexión externa debe estar abierta en modo Unit of Work");

                using (SqlCommand cmd = _externalConnection.CreateCommand())
                {
                    cmd.Transaction = _externalTransaction;
                    commandAction(cmd, _externalConnection, _externalTransaction);
                }
            }
            else
            {
                // Modo independiente: crear y gestionar propia conexión
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        commandAction(cmd, conn, null);
                    }
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta que retorna datos
        /// Gestiona automáticamente la conexión según el modo
        /// </summary>
        protected T ExecuteQuery<T>(Func<SqlCommand, SqlConnection, SqlTransaction, T> queryFunc)
        {
            if (_isUnitOfWorkMode)
            {
                // Modo Unit of Work: usar conexión y transacción externa
                if (_externalConnection == null)
                    throw new InvalidOperationException("Conexión externa es null en modo Unit of Work");

                if (_externalConnection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Conexión externa debe estar abierta en modo Unit of Work");

                using (SqlCommand cmd = _externalConnection.CreateCommand())
                {
                    cmd.Transaction = _externalTransaction;
                    return queryFunc(cmd, _externalConnection, _externalTransaction);
                }
            }
            else
            {
                // Modo independiente: crear y gestionar propia conexión
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        return queryFunc(cmd, conn, null);
                    }
                }
            }
        }
    }
}
