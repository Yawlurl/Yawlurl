using MediatR;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace YawlUrl.Application.ShortUrls.Queries
{
    public class GetShortUrlQuery : IRequest<ShortUrlDto>
    {
        [Required]
        [JsonProperty(PropertyName = "slug_key")]
        public string SlugKey { get; set; }

        [JsonProperty(PropertyName = "boost")]
        public bool Boost { get; set; }
    }
}
