using MediatR;
using System;

namespace PonyUrl.Application.ShortUrls.Commands.CreateShortUrl
{
    public class CreateShortUrlCommand : IRequest<Guid>
    {
        public string LongUrl { get; set; }
    }
}
