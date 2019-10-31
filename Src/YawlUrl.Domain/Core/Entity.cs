using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YawlUrl.Core;
using System;

namespace YawlUrl.Domain
{
    public abstract class Entity : IEntity<Guid>
    {
        [BsonId]
        [BsonElement]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    } 
}
