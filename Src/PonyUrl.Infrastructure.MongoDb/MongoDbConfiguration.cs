using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Domain.Interfaces;
using PonyUrl.Infrastructure.MongoDb.Repository;

namespace PonyUrl.Infrastructure.MongoDb
{
    public static class MongoDbConfiguration
    {
        private const string SectionName = nameof(MongoDbAppSettings); //"MongoDbAppSettings";

        public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        { 
            var settings = GetMongoDbAppSettings(configuration);


            services.AddScoped<IMongoDbSettings>(d => new MongoDbSettings(settings.ConnectionString));
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

            services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
        }

        public static MongoDbAppSettings GetMongoDbAppSettings(IConfiguration configuration)
        {
            var mongoDbSettingsSection = configuration.GetSection(SectionName);

           return mongoDbSettingsSection.Get<MongoDbAppSettings>();
        }

    }
}
