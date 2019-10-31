using YawlUrl.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Domain
{
    public interface ISlugRepository : IRepository<Slug, Guid>
    {
        Task<Slug> GetByKey(string key, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> IsExistByKey(string key, CancellationToken cancellationToken = default(CancellationToken));

    }
}
