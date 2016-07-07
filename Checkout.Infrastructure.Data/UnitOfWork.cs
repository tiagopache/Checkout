using Checkout.Infrastructure.Data.Base;
using Checkout.Infrastructure.Data.Contexts;
using Checkout.Infrastructure.Data.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;

namespace Checkout.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();

        Task<int> SaveAsync();
    }

    public interface IUnitOfWork<TEntity> : IUnitOfWork where TEntity : BaseEntity
    {
        IRepository<TEntity> Repository { get; set; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        [Dependency]
        internal IDbContext _context { get; set; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }


        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    _context.Dispose();

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class UnitOfWork<TEntity> : UnitOfWork, IUnitOfWork<TEntity> where TEntity : BaseEntity
    {
        [Dependency]
        public IRepository<TEntity> Repository { get; set; }
    }
}
