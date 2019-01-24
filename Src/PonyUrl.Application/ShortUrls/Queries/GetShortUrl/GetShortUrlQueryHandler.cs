using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Common;

namespace PonyUrl.Application.ShortUrls.Queries
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
