using YawlUrl.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Domain
{
    public interface ISlugManager
    {
        Task<Slug> Create(IUser user, string keyword, bool isRandom, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> IsExist(string keyword, CancellationToken cancellationToken = default(CancellationToken));

        Task<Guid> GetSlugIdByKeyword(string keyword, CancellationToken cancellationToken = default(CancellationToken));

        Task<Slug> GetSlugByKeyword(string keyword, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Activate(string keyword, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Deactivate(string keyword, CancellationToken cancellationToken = default(CancellationToken));

    }
}
