using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Infrastructure.MongoDb;
using PonyUrl.Infrastructure;
using System.IO;

namespace PonyUrl.Application.Test
{
    public class TestBase
    {
        public readonly MongoDbContext _mongoDbContext;
        public readonly ServiceCollection services;
        public readonly ServiceProvider serviceProvider;

        public TestBase()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json",
                                                    optional: false, reloadOnChange: true);

            IConfigurationRoot configurationRoot = builder.Build();

            services = new ServiceCollection();

            services.ConfigureGlobal(configurationRoot);

            serviceProvider = services.BuildServiceProvider();
        }

        public T That<T>()
        {
            return serviceProvider.GetService<T>();
        }

    }
}
