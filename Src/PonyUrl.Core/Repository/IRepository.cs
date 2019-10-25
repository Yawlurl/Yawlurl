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

        Task<TEntity> Get(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> BulkInsert(List<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken));

        Task<long> Count(CancellationToken cancellationToken = default(CancellationToken));

        Task<List<TEntity>> GetAllPagination(int pageIndex, int count, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> IsExist(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : IEntity<Guid>
    {

    }

}

