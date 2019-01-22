using PonyUrl.Common;
using PonyUrl.Core;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PonyUrl.Infrastructure
{
    public class ShortKeyManager : IShortKeyManager
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly ISettingManager _settingManager;

        public ShortKeyManager(IShortUrlRepository shortUrlRepository, ISettingManager settingManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _settingManager = settingManager;
        }

        public async Task<List<string>> ForbiddenWordsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _settingManager.ForbiddenWords();
        }

        public async Task<string> GenerateShortKeyRandomAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string shortKey = string.Empty;

            var forbiddenWords = await ForbiddenWordsAsync();

            bool isExist, containsForbiddenWord = false;

            //TODO:Add Forbidden words

            do
            {
                shortKey = RandomKey();

                if (Validation.IsNotNull(forbiddenWords) && forbiddenWords.Any())
                    containsForbiddenWord = forbiddenWords.Any(b => shortKey.Contains(b));

                isExist = await IsExistAsync(shortKey);

            } while (isExist || containsForbiddenWord);

            return shortKey;
        }


        public async Task<bool> IsExistAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _shortUrlRepository.IsExistAsync(shortKey, cancellationToken);
        }

        private string RandomKey(int size = 7)
        {

            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();

        }
    }
}
