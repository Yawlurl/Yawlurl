using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using YawlUrl.Common;
using YawlUrl.Domain;
using System.Linq;
using System;

namespace YawlUrl.Infrastructure.MongoDb
{
    public class ShortUrlRepository : MongoDbRepository<ShortUrl>, IShortUrlRepository
    {
        public ShortUrlRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {

        }

        public async Task<ShortUrl> GetBySlug(Guid slugId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await Collection.FindAsync(s => s.SlugId.Equals(slugId), null, cancellationToken)).FirstOrDefault();
        }

        public async Task<string> GetTargetUrlOnly(Guid slugId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await Collection.Find(s => s.SlugId.Equals(slugId)).
                Project<ShortUrl>(Builders<ShortUrl>.Projection.Include(s => s.LongUrl)).SingleOrDefaultAsync(cancellationToken)).LongUrl;
        }

        public async Task<bool> IsExistBySlug(Guid slugId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await Collection.Find(s => s.SlugId.Equals(slugId)).FirstOrDefaultAsync(cancellationToken)) != null;
        }
        /// <summary>
        /// Insert or Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShortUrl> InsertOrUpdate(ShortUrl entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await IsExistBySlug(entity.SlugId))
            {
                return await base.Update(entity, cancellationToken);
            }
            else
            {
                return await base.Insert(entity, cancellationToken);
            }
        }

        public async Task<List<ShortUrl>> GetAllShortUrlsByUser(string userId = "", CancellationToken cancelationToken = default)
        {
            return Check.IsNullOrEmpty(userId) ? await GetAll() : await GetMany(s => s.CreatedBy.Equals(userId));
        }

        public async Task<List<ShortUrl>> GetAllPaginationShortUrlsByUser(int pageIndex, int count, string userId = "", CancellationToken cancelationToken = default)
        {
            return Check.IsNullOrEmpty(userId) ? await GetAllPagination(pageIndex, count, cancelationToken) :
                await Task.FromResult(Collection.AsQueryable().Where(s => s.CreatedBy.Equals(userId)).Skip(pageIndex * count).Take(count).ToList());
        }

        public async Task<long> GetCountByUser(string userId = "", CancellationToken cancelationToken = default)
        {
            return Check.IsNullOrEmpty(userId) ? await Count(cancelationToken) : await Collection.CountDocumentsAsync(s => s.CreatedBy.Equals(userId));
        }

        public async Task<ShortUrl> GetShortUrlByLongUrl(string longUrl, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(s => s.LongUrl.Equals(longUrl)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
