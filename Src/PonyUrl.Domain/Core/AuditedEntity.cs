using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Core;
using PonyUrl.Domain.Entities;
using System;

namespace PonyUrl.Domain
{
    public abstract class AuditedEntity : Entity, IDateAudit
    {
        public AuditedEntity()
        {

        }

        [BsonElement("created_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }

        [BsonElement("creator")]
        public virtual User CreatedBy { get; set; }

        [BsonElement("updated_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedDate { get; set; }

        [BsonElement("modifier")]
        public virtual User UpdatedBy { get; set; }
    }
}
