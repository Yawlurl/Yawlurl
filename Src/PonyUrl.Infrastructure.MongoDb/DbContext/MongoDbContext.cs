using MongoDB.Driver;
using PonyUrl.Common;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        public MongoDbContext(string connectionString, string databaseName)
        {
            Validation.ArgumentNotNullOrEmpty(connectionString);
            Validation.ArgumentNotNullOrEmpty(databaseName);

            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }

        public MongoDbContext(MongoUrl mongoUrl)
        {
            Validation.ArgumentNotNull(mongoUrl);

            Client = new MongoClient(mongoUrl);
            Database = Client.GetDatabase(mongoUrl.DatabaseName);
        }

        public MongoDbContext(string mongoUrlString)
        {
            Validation.ArgumentNotNullOrEmpty(mongoUrlString);

            var builder = new MongoUrlBuilder(mongoUrlString);

            Client = new MongoClient(builder.ToMongoUrl());
            Database = Client.GetDatabase(builder.ToMongoUrl().DatabaseName);
        }

        public MongoDbContext(IMongoDatabase mongoDatabase)
        {
            Validation.ArgumentNotNull(mongoDatabase);

            Database = mongoDatabase;
            Client = mongoDatabase.Client;
        }

        public MongoDbContext(IMongoDbSettings mongoDbSettings) : this(mongoDbSettings.MongoUrl)
        {

        }

        public IMongoDatabase Database { get; private set; }

        public IMongoClient Client { get; private set; }

        public IMongoCollection<T> Collection<T>()
        {
            return Database.GetCollection<T>(MongoDbHelper.CreateCollectionName(typeof(T).Name));
        }
    }
}
