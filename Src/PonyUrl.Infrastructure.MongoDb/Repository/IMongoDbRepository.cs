using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using PonyUrl.Core;

namespace PonyUrl.Infrastructure.MongoDb
{
    public interface IMongoDbRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {
        IMongoCollection<TEntity> Collection { get; }
    }
}
