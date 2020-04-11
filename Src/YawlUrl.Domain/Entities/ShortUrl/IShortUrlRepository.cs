using YawlUrl.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Domain
{
    public interface IShortUrlRepository : IRepository<ShortUrl>
    {
        Task<bool> IsExistBySlug(Guid slugId, CancellationToken cancellationToken = default);

        Task<ShortUrl> InsertOrUpdate(ShortUrl entity, CancellationToken cancellationToken = default);

        Task<string> GetTargetUrlOnly(Guid slugId, CancellationToken cancellationToken = default);

        Task<ShortUrl> GetBySlug(Guid slugId, CancellationToken cancellationToken = default);

        Task<List<ShortUrl>> GetAllShortUrlsByUser(string userId, CancellationToken cancelationToken = default);

        Task<List<ShortUrl>> GetAllPaginationShortUrlsByUser(int pageIndex, int count, string userId = "", CancellationToken cancelationToken = default);

        Task<long> GetCountByUser(string userId, CancellationToken cancelationToken = default);

        Task<ShortUrl> GetShortUrlByLongUrl(string longUrl, CancellationToken cancellationToken = default);
    }
}
