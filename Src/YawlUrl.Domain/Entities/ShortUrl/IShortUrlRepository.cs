using YawlUrl.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Domain
{
    public interface IShortUrlRepository : IRepository<ShortUrl>
    {
        Task<bool> IsExistBySlug(Guid slugId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ShortUrl> InsertOrUpdate(ShortUrl entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetTargetUrlOnly(Guid slugId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ShortUrl> GetBySlug(Guid slugId, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<ShortUrl>> GetAllShortUrlsByUser(string userId, CancellationToken cancelationToken = default(CancellationToken));

        Task<List<ShortUrl>> GetAllPaginationShortUrlsByUser(int pageIndex, int count, string userId = "", CancellationToken cancelationToken = default(CancellationToken));

        Task<long> GetCountByUser(string userId, CancellationToken cancelationToken = default(CancellationToken));
    }
}
