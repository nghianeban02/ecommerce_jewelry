using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicCommerce.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        Task<T> AddAsync(T entity);
        int Save();
    }
}
