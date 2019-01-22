using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Core
{
    public interface IShortKeyManager
    {

        Task<string> GenerateShortKeyRandomAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> IsExistAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<string>> ForbiddenWordsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
