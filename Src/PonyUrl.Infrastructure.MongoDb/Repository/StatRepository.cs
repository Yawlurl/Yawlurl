using PonyUrl.Domain.Entities;
using PonyUrl.Domain.Interfaces;

namespace PonyUrl.Infrastructure.MongoDb.Repository
{
    public class StatRepository :  MongoDbRepository<Stat>, IStatRepository
    {
        public StatRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
