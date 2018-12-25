using System;
using MongoDB.Driver;
using PonyUrl.Common;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class MongoDbSettings : IMongoDbSettings
    {
        
        public MongoUrl MongoUrl {
            get
            {
                Validation.ArgumentNotNull(ConnectionString);

                var mongoUrlBuiler = new MongoUrlBuilder(ConnectionString);

                return mongoUrlBuiler.ToMongoUrl();
            }
        }

        public string ConnectionString { get; set; }

        public MongoDbSettings()
        {

        }

        public MongoDbSettings(string connectionString)
        {
            Validation.ArgumentNotNull(connectionString);

            ConnectionString = connectionString;

        }

             
    }
}
