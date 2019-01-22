using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Infrastructure.MongoDb;
using System.IO;
using PonyUrl.Infrastructure;

namespace PonyUrl.Application.Test.Infrastructure
{
    public class MongoDbContextFactory
    {
        private static IMongoDbSettings _dbSettings;
        private static ServiceProvider serviceProvider;
        private const string dbName = "PonyUrl_Int_Db";

        public static MongoDbContext Create(ServiceCollection services)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            services.ConfigureGlobal(configuration);

            serviceProvider = services.BuildServiceProvider();

            _dbSettings = serviceProvider.GetService<IMongoDbSettings>();

            MongoDbContext dbContext = new MongoDbContext(_dbSettings.MongoUrl);

            dbContext.Client.DropDatabase(dbName);

            return dbContext;
        }
    }
}
