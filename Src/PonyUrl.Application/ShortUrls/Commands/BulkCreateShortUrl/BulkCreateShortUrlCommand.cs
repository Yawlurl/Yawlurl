using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Application.ShortUrls.Commands.BulkCreateShortUrl
{
    public class BulkCreateShortUrlCommand : IRequest<List<ShortUrlDto>>
    {
        public List<string> LongUrls { get; set; }
    }
}
