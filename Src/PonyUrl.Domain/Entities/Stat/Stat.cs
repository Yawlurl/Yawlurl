using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Common;
using PonyUrl.Core;
using System;

namespace PonyUrl.Domain
{
    public class Stat : Entity, IDateAudit
    {
        public Stat()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [BsonRepresentation(BsonType.String)]
        public Guid ShortUrlId { get; set; }

        public string IpAddress { get; set; }

        public string Referer { get; set; }

        public string UserAgent { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedDate { get; set; }

        public Stat(Guid shortUrlId, string ipAddress, string referer, string userAgent) : this()
        {
            Check.ArgumentNotDefaultOrEmpty(shortUrlId);
            Check.ArgumentNotNullOrEmpty(ipAddress);

            ShortUrlId = shortUrlId;
            IpAddress = ipAddress;
            Referer = referer;
            UserAgent = userAgent;
        }
    }
}
