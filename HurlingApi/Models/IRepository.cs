using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> FindSingleAsync(Expression<Func<T, bool>> match);
        Task<bool> ExistAsync(Expression<Func<T, bool>> match);

        Task<int> UpdateAsync(T t);

        Task<int> InsertAsync(T t);

        Task<int> RemoveAsync(T t);
    }
}
