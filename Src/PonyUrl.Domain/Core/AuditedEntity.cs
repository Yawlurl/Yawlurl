using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Core;
using System;

namespace PonyUrl.Domain
{
    public abstract class AuditedEntity : Entity, IDateAudit
    {
        public AuditedEntity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }

        public virtual string CreatedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedDate { get; set; }

        public virtual string UpdatedBy { get; set; }
    }
}
