using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using PonyUrl.Domain.Interfaces;
using PonyUrl.Common;

namespace PonyUrl.Application.ShortUrls.Queries.GetShortUrl
{
    public class GetShortUrlQueryHandler : IRequestHandler<GetShortUrlQuery, ShortUrlViewModel>
    {
        IShortUrlRepository _shortUrlRepository;
        public GetShortUrlQueryHandler(IShortUrlRepository shortUrlRepository)
        {
            _shortUrlRepository = shortUrlRepository;
        }

        public async Task<ShortUrlViewModel> Handle(GetShortUrlQuery request, CancellationToken cancellationToken)
        {

            var entity = await _shortUrlRepository.GetAsync(request.Id);

            Validation.ArgumentNotNull(entity, "ShortUrl cannot find!");

           
            return new ShortUrlViewModel
            {
                Id = entity.Id,
                ShortKey = entity.ShortKey,
                LongUrl = entity.LongUrl,
                Hits = entity.Hits
            };

        }
    }
}
