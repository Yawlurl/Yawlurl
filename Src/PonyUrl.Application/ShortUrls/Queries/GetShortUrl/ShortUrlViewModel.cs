using PonyUrl.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace PonyUrl.Application.ShortUrls.Queries.GetShortUrl
{
    public class ShortUrlViewModel
    {
        public Guid Id { get; set; }
        public string ShortKey { get; set; }
        public string LongUrl { get; set; }
        public long Hits { get; set; }


        public static Expression<Func<ShortUrl, ShortUrlViewModel>> Map
        {
            get
            {
                return p => new ShortUrlViewModel
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
