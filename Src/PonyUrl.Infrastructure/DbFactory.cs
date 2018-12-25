using MongoDB.Driver;
using PonyUrl.Core;
using PonyUrl.Infrastructure.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure
{
    public class DbFactory<TDbContext> : IDbFactory<TDbContext> where TDbContext : class, IDbContext
    {
        public TDbContext Create(IDbSettings dbSettings)
        {

            if (dbSettings is IMongoDbSettings)
            {
                var mongoUrlBuilder = new MongoUrlBuilder(dbSettings.ConnectionString);

                return new MongoDbContext(mongoUrlBuilder.ToMongoUrl()) as TDbContext;
            }

            return default(TDbContext);
        }
    }
}
