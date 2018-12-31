using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Application.ShortUrls.Commands.DeleteShortUrl
{
    public class DeleteShortUrlCommand : IRequest 
    {
        public Guid Id { get; set; }
    }
}
