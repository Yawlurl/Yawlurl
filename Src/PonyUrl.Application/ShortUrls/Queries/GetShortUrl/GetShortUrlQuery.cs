using MediatR;
using System;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetShortUrlQuery : IRequest<ShortUrlViewModel>
    {
        public string ShortKey { get; set; }
    }
}
