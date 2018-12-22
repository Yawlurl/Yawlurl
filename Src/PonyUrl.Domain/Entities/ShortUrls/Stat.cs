using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Common;
using PonyUrl.Core;
using System;

namespace PonyUrl.Domain.Entities
{
    public class Stat : Entity, IDateAudit
    {
        [BsonElement("shorturl_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid ShortUrlId { get; set; }

        [BsonElement("ip_address")]
        public string IpAddress { get; set; }

        [BsonElement("referer")]
        public string Referer { get; set; }

        [BsonElement("user_agent")]
        public string UserAgent { get; set; }

        [BsonElement("created_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }

        [BsonElement("updated_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedDate { get; set; }

        public Stat()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public Stat(Guid shortUrlId, string ipAddress, string referer, string userAgent) : this()
        {
            Validation.ArgumentNotNullOrEmpty(shortUrlId);
            Validation.ArgumentNotNullOrEmpty(ipAddress);

            ShortUrlId = shortUrlId;
            IpAddress = ipAddress;
            Referer = referer;
            UserAgent = userAgent;
        }
    }
}
