using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Application.Test.Infrastructure;
using PonyUrl.Infrastructure.MongoDb;

namespace PonyUrl.Application.Test
{
    public class TestBase
    {
        public readonly MongoDbContext _mongoDbContext;
        public ServiceCollection services;
        public TestBase()
        {
            services = new ServiceCollection();

            _mongoDbContext = MongoDbContextFactory.Create(services); 
        }
 
    }
}
