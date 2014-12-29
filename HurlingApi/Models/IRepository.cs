using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HurlingApi.Models
{
    /// <summary>
    /// Generic repository per entity
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// select * from T
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// select * from T where match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> match);

        /// <summary>
        /// update T set T where T id ...
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T t);

        /// <summary>
        /// Insert into t where id ...
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> InsertAsync(T t);

        /// <summary>
        /// delete from t where t.Id ...
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> RemoveAsync(T t);

    }
}
