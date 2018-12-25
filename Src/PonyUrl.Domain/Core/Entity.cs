using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Core;
using System;

namespace PonyUrl.Domain
{
    public abstract class Entity : IEntity<Guid>
    {
        [BsonId]
        [BsonElement]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    } 
}
