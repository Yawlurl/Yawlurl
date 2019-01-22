using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Core;
using PonyUrl.Infrastructure.MongoDb;

namespace PonyUrl.Infrastructure
{
    public static class GlobalConfiguration
    {
        public static void ConfigureGlobal(this IServiceCollection services, IConfiguration configuration)
        {
            //Common
            services.AddSingleton(typeof(IDbFactory<>), typeof(DbFactory<>));

            //MongoDb Congiguration
            services.ConfigureMongoDb(configuration);

            //Managers
            services.AddScoped<IShortKeyManager, ShortKeyManager>();
            services.AddScoped<ISettingManager, SettingManager>();

        }
    }
}
