using MediatR;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class DeleteShortUrlCommand : IRequest<bool> 
    {
        [Required]
        [JsonProperty(PropertyName = "slug_key")]
        public string SlugKey { get; set; }
    }
}
