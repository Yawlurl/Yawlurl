using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommand : IRequest<Guid>
    {
        public string LongUrl { get; set; }

    }
}
