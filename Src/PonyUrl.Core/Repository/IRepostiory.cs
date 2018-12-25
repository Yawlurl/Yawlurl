using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Core
{
    public interface IRepostiory<TEntity>
    {
        IDbContext DbContext { get; }

        List<TEntity> GetAll();

        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        List<TEntity> GetMany(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken));

        long GetCount();

        Task<long> GetCountAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IRepository<TEntity, TKey> : IRepostiory<TEntity>
    {
        TEntity Get(TKey id);

        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        TEntity Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        bool Delete(TKey id);

        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
    }

}

