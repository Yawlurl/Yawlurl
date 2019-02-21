using MongoDB.Driver;
using PonyUrl.Core;
using PonyUrl.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class MongoDbRepository<TEntity> : IMongoDbRepository<TEntity> where TEntity : class, IEntity<Guid>
    {
        private readonly IMongoDbContext _mongoDbContext;

        public IMongoCollection<TEntity> Collection { get; private set; }

        public IDbContext DbContext => _mongoDbContext;

        public MongoDbRepository(IMongoDbContext mongoDbContext)
        {
            Check.ArgumentNotNull(mongoDbContext);

            _mongoDbContext = mongoDbContext;

            Collection = _mongoDbContext.Collection<TEntity>();
        }

        public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(id);
            DeleteResult deleteResult = await Collection.DeleteOneAsync(r => r.Id.Equals(id), cancellationToken);
            return deleteResult.DeletedCount > 0;
        }

        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Collection.AsQueryable().ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(id);

            return (await Collection.FindAsync(r => r.Id.Equals(id), null, cancellationToken)).FirstOrDefault();
        }


        public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.Collection.EstimatedDocumentCountAsync();
        }


        public virtual async Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await this.Collection.FindAsync(filter)).ToList();
        }


        public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(entity);

            await Collection.InsertOneAsync(entity);

            return await GetAsync(entity.Id);
        }


        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(entity);

            return await Collection.FindOneAndReplaceAsync(e => e.Id.Equals(entity.Id), entity, null, cancellationToken);
        }
    }
}
