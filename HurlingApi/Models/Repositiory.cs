using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    public class Repositiory<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public Repositiory()
        {
            _context = new HurlingModelContext();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync<T>();
        }


        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<int> UpdateAsync(T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }


        public async Task<int> InsertAsync(T t)
        {
            _context.Set<T>().Add(t);
            return await _context.SaveChangesAsync();
        }


        public async Task<int> RemoveAsync(T t)
        {
            _context.Entry(t).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }
    }
}