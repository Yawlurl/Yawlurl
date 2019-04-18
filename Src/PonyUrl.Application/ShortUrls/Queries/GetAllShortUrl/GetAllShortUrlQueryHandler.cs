using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetAllShortUrlQueryHandler : IRequestHandler<GetAllShortUrlQuery, ShortUrlListViewModel>
    {
        private readonly IShortUrlRepository _shortUrlRepository;

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
