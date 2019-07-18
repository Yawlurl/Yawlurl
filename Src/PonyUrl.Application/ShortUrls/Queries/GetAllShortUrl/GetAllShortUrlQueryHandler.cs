using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Core;
using System.Collections.Generic;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetAllShortUrlQueryHandler : IRequestHandler<GetAllShortUrlQuery, ShortUrlListViewModel>
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly ICacheManager _cacheManager;

        public GetAllShortUrlQueryHandler(IShortUrlRepository shortUrlRepository, ICacheManager cacheManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _cacheManager = cacheManager;
        }

        public async Task<ShortUrlListViewModel> Handle(GetAllShortUrlQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await _shortUrlRepository.GetCountAsync();

            var model = new ShortUrlListViewModel
            {
                TotalCount = totalCount
            };

            List<ShortUrl> list = new List<ShortUrl>();

            if (totalCount > 0 && request.Skip.HasValue && request.Limit.HasValue && request.Limit.Value > 0)
            {
                list = await _shortUrlRepository.GetAllPaginationAsync(request.Skip.Value, request.Limit.Value, cancellationToken);
            }
            else if (totalCount > 0)
            {
                list = await _shortUrlRepository.GetAllAsync(cancellationToken);
            }

            if (list.Any())
                model.ShortUrls = list.AsQueryable().Select(ShortUrlDto.Map);

            return model;
        }
    }
}
