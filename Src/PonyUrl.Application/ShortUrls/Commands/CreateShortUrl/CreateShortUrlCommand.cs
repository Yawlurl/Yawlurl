using MediatR;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommand : IRequest<ShortUrlDto>
    {
        [JsonProperty(PropertyName = "slug_key")]
        public string SlugKey { get; set; }

        [JsonProperty(PropertyName = "long_url")]
        public string LongUrl { get; set; }
    }
}
