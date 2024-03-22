using ElectronicCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ElectronicCommerce.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public IBaseRepository<T> GenericResponsitories<T>() where T : class
        {
            IBaseRepository<T> repo = new BaseRepository<T>(_context);
            return repo;
        }

        public void Save()
        {

        }
    }
}
