using PonyUrl.Common;
using PonyUrl.Domain;
using System;
using System.Linq.Expressions;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class ShortUrlViewModel
    {
        public Guid Id { get; set; }
        public string ShortKey { get; set; }
        public string LongUrl { get; set; }
        public long Hits { get; set; }


        public void MapFromEntity(ShortUrl entity)
        {
            Check.ArgumentNotNull(entity);

            Id = entity.Id;
            ShortKey = entity.ShortKey;
            LongUrl = entity.LongUrl;
            Hits = entity.Hits;
        }
        
        
    }
}
