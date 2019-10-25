using PonyUrl.Domain;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class StatRepository :  MongoDbRepository<Stat>, IStatRepository
    {
        public StatRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
