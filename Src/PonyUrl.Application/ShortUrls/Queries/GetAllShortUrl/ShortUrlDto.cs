using PonyUrl.Domain.Entities; 
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PonyUrl.Application.ShortUrls.Queries.GetAllShortUrl
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
