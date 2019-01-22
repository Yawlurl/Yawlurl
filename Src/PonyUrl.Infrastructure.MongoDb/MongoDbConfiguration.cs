using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Domain;
using PonyUrl.Infrastructure.MongoDb.Repository;

namespace PonyUrl.Infrastructure.MongoDb
{
    public static class MongoDbConfiguration
    {
        private const string SectionName = nameof(MongoDbAppSettings); //"MongoDbAppSettings";

        public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSettingsSection = configuration.GetSection(SectionName);

            var settings = mongoDbSettingsSection.Get<MongoDbAppSettings>();


            services.AddScoped<IMongoDbSettings>(d => new MongoDbSettings(settings.ConnectionString));
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

            services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IStatRepository, StatRepository>();
        }

    }
}
