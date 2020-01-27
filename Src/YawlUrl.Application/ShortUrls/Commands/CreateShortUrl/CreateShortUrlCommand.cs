using MediatR;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace YawlUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommand : IRequest<ShortUrlDto>
    {
        [JsonProperty(PropertyName = "slug_key")]
        public string SlugKey { get; set; }

        [JsonProperty(PropertyName = "long_url")]
        public string LongUrl { get; set; }

        [JsonIgnore]
        public bool IsRouter { get; set; }
    }
}
