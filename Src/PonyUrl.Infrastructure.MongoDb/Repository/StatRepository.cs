using PonyUrl.Domain;

namespace PonyUrl.Infrastructure.MongoDb.Repository
{
    public class StatRepository :  MongoDbRepository<Stat>, IStatRepository
    {
        public StatRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
