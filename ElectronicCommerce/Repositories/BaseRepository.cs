using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicCommerce.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DatabaseContext _context = null;
        private DbSet<T> table = null;

        public BaseRepository()
        {
            this._context = new DatabaseContext();
            table = _context.Set<T>();
        }

        public BaseRepository(DatabaseContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public async Task<T> AddAsync(T entity)
        {
            _context.Add(entity);
            return entity;
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
