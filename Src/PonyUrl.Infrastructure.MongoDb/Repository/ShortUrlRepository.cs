using PonyUrl.Domain.Entities;
using PonyUrl.Domain.Interfaces;

namespace PonyUrl.Infrastructure.MongoDb.Repository
{
    public class ShortUrlRepository : MongoDbRepository<ShortUrl>, IShortUrlRepository
    {
        public ShortUrlRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
