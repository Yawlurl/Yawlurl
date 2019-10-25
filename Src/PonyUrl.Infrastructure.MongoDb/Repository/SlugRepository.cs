using PonyUrl.Domain;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using PonyUrl.Core;
using System;
using PonyUrl.Common;
using MongoDB.Bson;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class SlugRepository : MongoDbRepository<Slug>, ISlugRepository
    {
        public SlugRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }

        /// <summary>
        /// Get Slug by key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Slug> GetByKey(string key, CancellationToken cancellationToken = default)
        {

            var firstCondition = (await Task.FromResult(Collection.AsQueryable().Where(s => s.IsRandom).Any(s => s.Key == key)));

            if (firstCondition) return (await Task.FromResult(Collection.AsQueryable().Where(s => s.IsRandom).FirstOrDefault(s => s.Key == key))); ;

            var secondCondition = (await Task.FromResult(Collection.AsQueryable().Where(s => !s.IsRandom).Any(s => s.Key == key.Trim().ToLower())));

            if (secondCondition) return (await Task.FromResult(Collection.AsQueryable().Where(s => !s.IsRandom).FirstOrDefault(s => s.Key == key.Trim().ToLower()))); ;

            return new NullSlug();
        }

        /// <summary>
        /// Comparison with Key not id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<bool> IsExist(Slug entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var firstCondition = (await Task.FromResult(Collection.AsQueryable().Where(s => s.IsRandom).Any(s => s.Key == entity.Key)));

            if (firstCondition) return true;

            var secondCondition = (await Task.FromResult(Collection.AsQueryable().Where(s => !s.IsRandom).Any(s => s.Key == entity.Key.Trim().ToLower())));

            if (secondCondition) return true;

            return false;
        }
        /// <summary>
        /// Comparison with Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsExistByKey(string key, CancellationToken cancellationToken = default)
        {
            var firstCondition = (await Task.FromResult(Collection.AsQueryable().Where(s => s.IsRandom).Any(s=> s.Key == key)));

            if (firstCondition) return true;

            var secondCondition = (await Task.FromResult(Collection.AsQueryable().Where(s => !s.IsRandom).Any(s => s.Key == key.Trim().ToLower())));

            if (secondCondition) return true;

            return false;
        }
    }
}
