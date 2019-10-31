using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using YawlUrl.Core;

namespace YawlUrl.Infrastructure.MongoDb
{
    public interface IMongoDbSettings : IDbSettings
    {
         MongoUrl MongoUrl { get; }
    }
}
