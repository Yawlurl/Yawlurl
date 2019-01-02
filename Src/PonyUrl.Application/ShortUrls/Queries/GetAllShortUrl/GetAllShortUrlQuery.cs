using MediatR;

namespace PonyUrl.Application.ShortUrls.Queries.GetAllShortUrl
{
    public class GetAllShortUrlQuery : IRequest<ShortUrlListViewModel>
    {
    }
}
