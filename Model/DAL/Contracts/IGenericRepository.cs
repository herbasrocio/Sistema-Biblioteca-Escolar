using System;
using System.Collections.Generic;

namespace DAL.Contracts
{
    public interface IGenericRepository<T> where T : class, new()
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        List<T> GetAll();
    }
}
