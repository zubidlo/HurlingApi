using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T">A repository entity type.</typeparam>
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary></summary>
        /// <returns>List of requested entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary></summary>
        /// <param name="match">Linq Expression.</param>
        /// <returns>Single requested entity.</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> match);

        /// <summary></summary>
        /// <param name="t">A repository entity type.</param>
        /// <returns>Integer result code.</returns>
        Task<int> UpdateAsync(T t);

        /// <summary></summary>
        /// <param name="t">A repository entity type.</param>
        /// <returns>Integer result code.</returns>
        Task<int> InsertAsync(T t);

        /// <summary></summary>
        /// <param name="t">A repository entity type.</param>
        /// <returns>Integer result code.</returns>
        Task<int> RemoveAsync(T t);
    }
}
