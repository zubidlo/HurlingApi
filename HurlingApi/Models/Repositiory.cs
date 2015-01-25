using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    /// <summary></summary>
    /// <typeparam name="T">A repository entity type.</typeparam>
    public class Repositiory<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        bool _disposed;

        /// <summary></summary>
        public Repositiory(DbContext context)
        {
            _context = context;
        }

        /// <summary></summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        ~Repositiory()
        {
            Dispose(false);
        }

        /// <summary></summary>
        /// <returns>List of requested entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync<T>();
        }

        /// <summary></summary>
        /// <param name="match">Linq Expression</param>
        /// <returns>Single requested entity.</returns>
        /// <exception cref="System.InvalidOperationException">If more than one resouce found in the repository.</exception>
        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            T t = null;
            try
            {
                t = await _context.Set<T>().SingleOrDefaultAsync(match);
                return t;
            }
            catch (InvalidOperationException e)
            {
                throw new Exception("More than one requested " + t.GetType().Name + " found in the repository.", e);
            }
        }

        /// <summary></summary>
        /// <param name="t">A repository entity type.</param>
        /// <returns>Integer result code.</returns>
        /// <exception cref="System.InvalidOperationException">Error occured during repository modification.</exception>
        public async Task<int> UpdateAsync(T t)
        {
            try
            {
                _context.Entry(t).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("An error occured during " + t.GetType().Name + " repository modification.", e);
            }
        }

        /// <summary></summary>
        /// <param name="t">A repository entity type.</param>
        /// <returns>Integer result code.</returns>
        /// <exception cref="System.InvalidOperationException">Error occured during adding to repository.</exception>
        public async Task<int> InsertAsync(T t)
        {
            try
            {
                _context.Set<T>().Add(t);
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error occured during adding " + t.GetType().Name + " to repository.", e);
            }
        }

        /// <summary></summary>
        /// <param name="t">A repository entity type.</param>
        /// <returns>Integer result code.</returns>
        /// <exception cref="System.InvalidOperationException">Error occured during deleting from repository.</exception>
        public async Task<int> RemoveAsync(T t)
        {
            try
            {
                _context.Entry(t).State = EntityState.Deleted;
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error occured during deleting " + t.GetType().Name + " from repository.", e);
            }
        }
    }
}