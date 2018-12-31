 using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain.Interfaces;

namespace PonyUrl.Application.ShortUrls.Queries.GetAllShortUrl
{
    public class GetAllShortUrlQueryHandler : IRequestHandler<GetAllShortUrlQuery, ShortUrlListViewModel>
    {
        IShortUrlRepository _shortUrlRepository;
        public GetAllShortUrlQueryHandler(IShortUrlRepository shortUrlRepository)
        {
            _shortUrlRepository = shortUrlRepository;
        }

        public async Task<ShortUrlListViewModel> Handle(GetAllShortUrlQuery request, CancellationToken cancellationToken)
        {

            var list = await _shortUrlRepository.GetAllAsync();

            var model = new ShortUrlListViewModel
            {
                 ShortUrls = list.AsQueryable().Select(ShortUrlDto.Map)
            };

            return model;


        }
    }
}
