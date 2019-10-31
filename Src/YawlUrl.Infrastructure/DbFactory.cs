using MongoDB.Driver;
using YawlUrl.Core;
using YawlUrl.Infrastructure.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Infrastructure
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
