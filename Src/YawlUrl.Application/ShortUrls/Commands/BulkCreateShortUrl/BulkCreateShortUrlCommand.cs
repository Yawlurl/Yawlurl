using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Application.ShortUrls.Commands
{
    public class BulkCreateShortUrlCommand : IRequest<List<ShortUrlDto>>
    {
        [JsonProperty(PropertyName = "long_urls")]
        public List<string> LongUrls { get; set; }
    }
}
