using MongoDB.Driver;
using YawlUrl.Core;
using YawlUrl.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace YawlUrl.Infrastructure.MongoDb
{
    public class MongoDbRepository<TEntity> : IMongoDbRepository<TEntity> where TEntity : class, IEntity<Guid>
    {
        private readonly IMongoDbContext _mongoDbContext;

        public IMongoCollection<TEntity> Collection { get; private set; }

        public IDbContext DbContext => _mongoDbContext;

        public IMongoClient Client { get; private set; }

        public MongoDbRepository(IMongoDbContext mongoDbContext)
        {
            Check.ArgumentNotNull(mongoDbContext);

            _mongoDbContext = mongoDbContext;

            Collection = _mongoDbContext.Collection<TEntity>();

            Client = mongoDbContext.Client;
        }

        public virtual async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(id);
            DeleteResult deleteResult = await Collection.DeleteOneAsync(r => r.Id.Equals(id), cancellationToken);
            return deleteResult.DeletedCount > 0;
        }

        public virtual async Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Collection.AsQueryable().ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> Get(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(id);

            return (await Collection.FindAsync(r => r.Id.Equals(id), null, cancellationToken)).FirstOrDefault();
        }


        public virtual async Task<long> Count(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Collection.EstimatedDocumentCountAsync(null, cancellationToken);
        }


        public virtual async Task<List<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await Collection.FindAsync(filter)).ToList();
        }


        public virtual async Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(entity);

            await Collection.InsertOneAsync(entity, null, cancellationToken);

            return entity;
        }


        public virtual async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(entity);

            return await Collection.FindOneAndReplaceAsync(e => e.Id.Equals(entity.Id), entity, null, cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetAllPagination(int pageIndex, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.FromResult(Collection.AsQueryable().Skip(pageIndex * count).Take(count).ToList());
        }

        public virtual async Task<List<TEntity>> BulkInsert(List<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(entities);

            await Collection.InsertManyAsync(entities, null, cancellationToken);

            return entities;
        }

        public virtual async Task<bool> IsExist(TEntity entity, CancellationToken cancellationToken = default)
        {
            Check.ArgumentNotNull(entity);

            return (await Collection.FindAsync(s => s.Id.Equals(entity.Id))).Any();
        }
    }
}
