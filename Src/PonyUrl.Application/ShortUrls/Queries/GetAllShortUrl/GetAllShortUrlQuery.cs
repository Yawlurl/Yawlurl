using MediatR;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetAllShortUrlQuery : IRequest<ShortUrlListViewModel>
    {
        public int? Skip { get; set; }
        public int? Limit { get; set; }

        public GetAllShortUrlQuery()
        {

        }

        public GetAllShortUrlQuery(int skip, int limit)
        {
            Skip = skip;
            Limit = limit;
        }
    }
}
