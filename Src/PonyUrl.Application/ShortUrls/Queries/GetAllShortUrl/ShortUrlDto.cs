using PonyUrl.Domain;
using System;
using System.Linq.Expressions;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class ShortUrlDto
    {
        public Guid Id { get; set; }
        public string ShortKey { get; set; }
        public string LongUrl { get; set; }
        public long Hits { get; set; }

        public static Expression<Func<ShortUrl, ShortUrlDto>> Map
        {
            get
            {
                return p => new ShortUrlDto
                {
                    Id = p.Id,
                    ShortKey = p.ShortKey,
                    LongUrl = p.LongUrl,
                    Hits = p.Hits
                };
            }
        }
    }
}
