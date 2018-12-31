using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Infrastructure.MongoDb;
using System.IO;

namespace PonyUrl.Application.Test.Infrastructure
{
    public  class MongoDbContextFactory
    {
        private static IMongoDbSettings _dbSettings;
        private static  ServiceProvider serviceProvider;
        private static string dbName = "PonyUrl_Int_Db";

        public static MongoDbContext Create(ServiceCollection services)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            
            services.ConfigureMongoDb(builder.Build());

            serviceProvider = services.BuildServiceProvider();

            _dbSettings = serviceProvider.GetService<IMongoDbSettings>();

            MongoDbContext dbContext = new MongoDbContext(_dbSettings.MongoUrl);

            dbContext.Client.DropDatabase(dbName);

            return dbContext; 
        }
    }
}
