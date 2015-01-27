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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Entity<T> : IEntity<T> where T : class
    {
        private readonly DbContext _context;
        
        bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Entity(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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

        /// <summary>
        /// 
        /// </summary>
        ~Entity()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> items = await _context.Set<T>().ToListAsync<T>();
            return items;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> match)
        {
            T singleItem = null;
            try
            {
                singleItem = await _context.Set<T>().SingleOrDefaultAsync(match);
                return singleItem;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("More than one requested " + singleItem.GetType().Name + " found in the repository.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> match)
        {
            bool exist = await _context.Set<T>().AnyAsync(match);
            return exist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<int> UpdateAsync(T t)
        {
            try
            {
                _context.Entry(t).State = EntityState.Modified;
                int result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                throw new Exception("An error occured during " + t.GetType().Name + " repository modification.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<int> InsertAsync(T t)
        {
            try
            {
                _context.Set<T>().Add(t);
                int result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Error occured during adding " + t.GetType().Name + " to repository.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<int> RemoveAsync(T t)
        {
            try
            {
                _context.Entry(t).State = EntityState.Deleted;
                int result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Error occured during deleting " + t.GetType().Name + " from repository.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            try {
                int result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}