using MongoDB.Driver;
using System;
using PonyUrl.Core;

namespace PonyUrl.Infrastructure.MongoDb
{
    public interface IMongoDbContext : IDbContext
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        IMongoCollection<T> Collection<T>();
    }
}

