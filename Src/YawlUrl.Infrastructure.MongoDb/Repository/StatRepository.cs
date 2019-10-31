using YawlUrl.Domain;

namespace YawlUrl.Infrastructure.MongoDb
{
    public class StatRepository :  MongoDbRepository<Stat>, IStatRepository
    {
        public StatRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
