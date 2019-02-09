using MediatR;
using System;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class DeleteShortUrlCommand : IRequest 
    {
        public Guid Id { get; set; }
    }
}
