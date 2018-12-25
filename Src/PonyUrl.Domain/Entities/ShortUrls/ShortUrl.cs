using MongoDB.Bson.Serialization.Attributes;

namespace PonyUrl.Domain.Entities
{
    public class ShortUrl : AuditedEntity
    {
        [BsonElement("short_key")]
        public string ShortKey { get; set; }

        [BsonElement("long_url")]
        public string LongUrl { get; set; }

        [BsonElement("hits")]
        public long Hits { get; set; }
    }
}
