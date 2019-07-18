using PonyUrl.Infrastructure.Redis.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Core;

namespace PonyUrl.Infrastructure.Redis
{
    public static class RedisConfiguration
    {
        private const string sectionName = "Redis";

        public static void ConfigureRedisDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisAppSettings>(configuration.GetSection(sectionName));

            var redisConfig = new RedisAppSettings();

            configuration.GetSection(sectionName).Bind(redisConfig);

            services.AddSingleton<ICacheManager>(new RedisManager(redisConfig.Host, redisConfig.DefaultDb, redisConfig.Port));
        }
    }
}
