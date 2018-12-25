using MongoDB.Driver;
using PonyUrl.Core;
using PonyUrl.Common;
using System;

namespace PonyUrl.Infrastructure.MongoDb
{
    public class MongoDbManager: IDbManager
    {
        public bool ConnectionAvailable(IDbSettings dbSettings)
        {
            if(!(dbSettings is IMongoDbSettings))
            {
                throw new ArgumentException("DbSettings is not MongoDbSettings");
            }

            bool result = false;

            IMongoDbSettings mongoDbSettings = (IMongoDbSettings)dbSettings;

            IMongoClient mongoClient = null;

            try
            {
                
                mongoClient = new MongoClient(mongoDbSettings.MongoUrl);

                result = Validation.IsNotNull(mongoClient.GetDatabase(mongoDbSettings.MongoUrl.DatabaseName));

            }
            finally
            {
                
            }

            return result;
        }

       
    }
}
