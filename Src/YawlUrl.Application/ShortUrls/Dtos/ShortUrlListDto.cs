using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Application
{
    public class ShortUrlListDto
    {
        [JsonProperty(PropertyName = "total_count")]
        public long TotalCount { get; set; }

        [JsonProperty(PropertyName = "short_urls")]
        public IEnumerable<ShortUrlDto> ShortUrls { get; set; }

        public ShortUrlListDto()
        {
            ShortUrls = new List<ShortUrlDto>();
        }
    }
}
