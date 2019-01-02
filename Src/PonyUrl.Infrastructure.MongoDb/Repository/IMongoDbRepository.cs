using System;
using System.Collections.Generic;
using System.Text;
using PonyUrl.Core;

namespace PonyUrl.Infrastructure.MongoDb
{
    public interface IMongoDbRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {
        
    }
}
