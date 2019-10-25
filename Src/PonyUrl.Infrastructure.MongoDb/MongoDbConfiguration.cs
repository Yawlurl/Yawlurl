using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace PonyUrl.Infrastructure.MongoDb
{
    public static class MongoDbConfiguration
    {
        private const string MONGODB_API_CONNECTION = "MongoDbApi";

        public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddScoped<IMongoDbSettings>(d => new MongoDbSettings(configuration.GetConnectionString(MONGODB_API_CONNECTION)));
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

        
        }

  
    }
}
