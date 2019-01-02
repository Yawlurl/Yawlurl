using PonyUrl.Infrastructure.Redis.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Infrastructure.Redis.Cache;

namespace PonyUrl.Infrastructure.Redis.Configurations
{
    public static class RedisConfiguration
    {
        private const string sectionName = "Redis";

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        { 
            services.Configure<RedisAppSettings>(configuration.GetSection(sectionName)); 
            var redisConfig = new RedisAppSettings();
            configuration.GetSection(sectionName).Bind(redisConfig);

            services.AddSingleton<RedisCacheProvider>(new RedisCacheProvider(redisConfig.Host)); 
        }
    }
}
