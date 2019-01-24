using PonyUrl.Core;
using PonyUrl.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Domain
{
    public interface IShortUrlRepository : IRepository<ShortUrl>
    {
        Task<bool> IsExistAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetLongUrlOnlyAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken));

        Task<ShortUrl> GetByShortKeyAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken));
    }
}
