using PonyUrl.Domain.Common;

namespace PonyUrl.Domain.Entities.ShortUrls
{
    public class ShortUrl : AuditedEntity
    {
        public string Code { get; set; }
        public string LongUrl { get; set; }
        public long Hits { get; set; }
    }
}
