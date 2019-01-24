using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, TKey> 
    {
        IDbContext DbContext { get; }

        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken));

        Task<long> GetCountAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : IEntity<Guid>
    {

    }

}

