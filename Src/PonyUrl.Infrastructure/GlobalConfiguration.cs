using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Core;
using System;
using System.Collections.Generic;
using System.Text;
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


        }
    }
}
