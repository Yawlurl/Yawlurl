using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using PonyUrl.Infrastructure.MongoDb;

namespace PonyUrl.Integration.Test
{
    public abstract class BaseTest
    {
        public ServiceProvider ServiceProvider;
        public IConfigurationRoot ConfigurationRoot;
        public ServiceCollection Services;

        public BaseTest()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            ConfigurationRoot = builder.Build();

            Services = new ServiceCollection();

            Services.ConfigureMongoDb(ConfigurationRoot);

            ServiceProvider = Services.BuildServiceProvider();
        }
    }
}
