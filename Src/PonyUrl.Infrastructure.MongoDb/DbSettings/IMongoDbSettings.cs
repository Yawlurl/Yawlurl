using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using PonyUrl.Core;

namespace PonyUrl.Infrastructure.MongoDb
{
    public interface IMongoDbSettings : IDbSettings
    {
         MongoUrl MongoUrl { get; }
    }
}
