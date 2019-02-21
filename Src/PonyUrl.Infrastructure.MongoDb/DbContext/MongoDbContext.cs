using MongoDB.Driver;
using PonyUrl.Common;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        public MongoDbContext(string connectionString, string databaseName)
        {
            Check.ArgumentNotNullOrEmpty(connectionString);
            Check.ArgumentNotNullOrEmpty(databaseName);

            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }

        public MongoDbContext(MongoUrl mongoUrl)
        {
            Check.ArgumentNotNull(mongoUrl);

            Client = new MongoClient(mongoUrl);
            Database = Client.GetDatabase(mongoUrl.DatabaseName);
        }

        public MongoDbContext(string mongoUrlString)
        {
            Check.ArgumentNotNullOrEmpty(mongoUrlString);

            var builder = new MongoUrlBuilder(mongoUrlString);

            Client = new MongoClient(builder.ToMongoUrl());
            Database = Client.GetDatabase(builder.ToMongoUrl().DatabaseName);
        }

        public MongoDbContext(IMongoDatabase mongoDatabase)
        {
            Check.ArgumentNotNull(mongoDatabase);

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
