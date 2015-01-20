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

        /// <summary></summary>
        public Repositiory(DbContext context)
        {
            _context = context;
        }

        /// <summary></summary>
        public void Dispose()
        {
            if (_context != null) _context.Dispose();
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
        /// <exception cref="System.InvalidOperationException">More than one resouces found in the repository.</exception>
        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            T t;
            try
            {
                t = await _context.Set<T>().SingleOrDefaultAsync(match);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("More than one resouces found in the repository.");
            }
            return t;
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
                throw new InvalidOperationException("Error occured during repository modification.");
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
                throw new InvalidOperationException("Error occured during adding to repository.");
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
                throw new InvalidOperationException("Error occured during deleting from repository.");
            }
        }
    }
}