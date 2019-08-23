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
            Check.That<DomainException>(Check.IsNotNullOrEmpty(longUrl), "longUrl is required!");

            LongUrl = longUrl;
        }

        public void Boost()
        {
            Hits += 1;
        }

        public void ResetBoost()
        {
            Hits = 0;
        }
    }
}
