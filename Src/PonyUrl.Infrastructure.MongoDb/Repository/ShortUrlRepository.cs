using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using PonyUrl.Common;
using PonyUrl.Domain;

namespace PonyUrl.Infrastructure.MongoDb.Repository
{
    public class ShortUrlRepository : MongoDbRepository<ShortUrl>, IShortUrlRepository
    {
        public ShortUrlRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }

        public async Task<ShortUrl> GetByShortKeyAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            Validation.ArgumentNotNullOrEmpty(shortKey);

            return (await Collection.FindAsync(s => s.ShortKey.Equals(shortKey), null, cancellationToken)).FirstOrDefault();
        }

        public async Task<string> GetLongUrlOnlyAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            var condition = Builders<ShortUrl>.Filter.Eq(s => s.ShortKey, shortKey);

            var fields = Builders<ShortUrl>.Projection.Include(s => s.LongUrl);

            return (await Collection.Find(condition).Project<ShortUrl>(fields).SingleOrDefaultAsync(cancellationToken)).LongUrl;
        }

        public async Task<bool> IsExistAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await Collection.Find(s => s.ShortKey.Equals(shortKey)).FirstOrDefaultAsync(cancellationToken)) != null;
        }

        public async override Task<ShortUrl> InsertAsync(ShortUrl entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await IsExistAsync(entity.ShortKey))
            {
                throw new DomainException("This shortkey already is exist.");
            }

            return await base.InsertAsync(entity, cancellationToken);
        }
    }
}
