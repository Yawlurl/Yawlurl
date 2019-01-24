using MediatR;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetAllShortUrlQuery : IRequest<ShortUrlListViewModel>
    {
    }
}
