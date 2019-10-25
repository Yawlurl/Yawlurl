using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Core;
using PonyUrl.Domain;
using PonyUrl.Infrastructure.MongoDb;

namespace PonyUrl.Infrastructure
{
    /// <summary>
    /// All configurations 
    /// </summary>
    public static class GlobalConfiguration
    {
        public static void ConfigureGlobal(this IServiceCollection services, IConfiguration configuration)
        {
            //Common
            services.AddSingleton(typeof(IDbFactory<>), typeof(DbFactory<>));

            //MongoDb Congiguration
            services.ConfigureMongoDb(configuration);

            //Managers
            services.AddScoped<ISlugManager, SlugManager>();
            services.AddScoped<ISettingManager, SettingManager>();

            //Repository
            services.AddTransient<IShortUrlRepository, ShortUrlRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            services.AddTransient<IStatRepository, StatRepository>();
            services.AddTransient<ISlugRepository, SlugRepository>();
        }
    }
}
