using MongoDB.Bson.Serialization.Attributes;
using YawlUrl.Core;
using System;

namespace YawlUrl.Domain
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
