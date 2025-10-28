using System;

namespace DAL.Contracts
{
    /// <summary>
    /// Patrón Unit of Work para gestionar transacciones que involucran múltiples repositorios
    /// Garantiza atomicidad: todas las operaciones se confirman juntas o se revierten juntas
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Préstamos
        /// </summary>
        IPrestamoRepository Prestamos { get; }

        /// <summary>
        /// Repositorio de Ejemplares
        /// </summary>
        IEjemplarRepository Ejemplares { get; }

        /// <summary>
        /// Repositorio de Devoluciones
        /// </summary>
        IDevolucionRepository Devoluciones { get; }

        /// <summary>
        /// Repositorio de Materiales
        /// </summary>
        IMaterialRepository Materiales { get; }

        /// <summary>
        /// Repositorio de Alumnos
        /// </summary>
        IAlumnoRepository Alumnos { get; }

        /// <summary>
        /// Inicia una transacción de base de datos
        /// Todas las operaciones subsecuentes compartirán esta transacción
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Confirma todas las operaciones realizadas en la transacción
        /// </summary>
        void Commit();

        /// <summary>
        /// Revierte todas las operaciones realizadas en la transacción
        /// </summary>
        void Rollback();

        /// <summary>
        /// Indica si hay una transacción activa
        /// </summary>
        bool HasActiveTransaction { get; }
    }
}
