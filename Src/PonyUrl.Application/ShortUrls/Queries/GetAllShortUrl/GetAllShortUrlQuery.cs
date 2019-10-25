using MediatR;
using Newtonsoft.Json;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetAllShortUrlQuery : IRequest<ShortUrlListDto>
    {
        [JsonProperty(PropertyName = "index")]
        public int? Index { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int? Limit { get; set; }

        public GetAllShortUrlQuery()
        {

        }

        public GetAllShortUrlQuery(int? index, int? limit)
        {
            Index = index;
            Limit = limit;
        }
    }
}
