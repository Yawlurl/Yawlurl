using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommand : IRequest<ShortUrlDto>
    {
        public string LongUrl { get; set; }
    }
}
