using MongoDB.Driver;
using System;
using YawlUrl.Core;

namespace YawlUrl.Infrastructure.MongoDb
{
    public interface IMongoDbContext : IDbContext
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        IMongoCollection<T> Collection<T>();
    }
}

