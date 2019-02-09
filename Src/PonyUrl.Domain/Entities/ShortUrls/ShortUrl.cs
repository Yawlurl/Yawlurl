using MongoDB.Bson.Serialization.Attributes;
using PonyUrl.Common;

namespace PonyUrl.Domain
{
    public class ShortUrl : AuditedEntity
    {
        [BsonElement("short_key")]
        public string ShortKey { get; set; }

        [BsonElement("long_url")]
        public string LongUrl { get; private set; }

        [BsonElement("hits")]
        public long Hits { get; set; }

        public ShortUrl(string longUrl)
        {
            Validation.ArgumentNotUrl(longUrl);

            LongUrl = longUrl;
        }
    }
}
