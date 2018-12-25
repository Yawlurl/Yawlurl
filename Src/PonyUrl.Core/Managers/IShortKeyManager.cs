using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Core
{
    public interface IShortKeyManager
    {
        string GenerateShortKeyRandom();

        Task<string> GenerateShortKeyRandomAsync(CancellationToken cancellationToken = default(CancellationToken));

        string GenerateShortKey(string text);

        Task<string> GenerateSlugAsync(string text, CancellationToken cancellationToken = default(CancellationToken));

        bool IsExist(string shortKey);

        Task<bool> IsExistAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken));

        List<string> ForbiddenWords();

        Task<List<string>> ForbiddenWordsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
