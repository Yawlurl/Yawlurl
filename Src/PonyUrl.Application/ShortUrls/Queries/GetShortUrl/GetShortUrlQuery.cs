using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Application.ShortUrls.Queries.GetShortUrl
{
    public class GetShortUrlQuery : IRequest<ShortUrlViewModel>
    {
        public Guid Id { get; set; }
    }
}
