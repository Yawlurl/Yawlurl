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
    /// <summary>
    /// 
    /// </summary>
    public class ShortKeyManager : IShortKeyManager
    {
        private readonly ISettingManager _settingManager;
        private readonly ICacheManager _cacheManager;
        public ShortKeyManager(ICacheManager cacheManager,
                               ISettingManager settingManager)
        {
            _settingManager = settingManager;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<string>> ForbiddenWordsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _settingManager.ForbiddenWords();
        }

        /// <summary>
        /// Generate unique shortkey
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GenerateShortKeyRandomAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string shortKey = string.Empty;

            var forbiddenWords = await ForbiddenWordsAsync();

            bool isExist, containsForbiddenWord = false;

            do
            {
                shortKey = RandomKey();

                if (Check.IsNotNull(forbiddenWords) && forbiddenWords.Any())
                    containsForbiddenWord = forbiddenWords.Any(b => shortKey.Contains(b));

                isExist = await IsExistAsync(shortKey);

            } while (isExist || containsForbiddenWord);

            //Create ShortKey In Cache Db
            await _cacheManager.SetUrl(shortKey, string.Empty);

            return shortKey;
        }

        /// <summary>
        /// Check key if exist in cache collection
        /// </summary>
        /// <param name="shortKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(string shortKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _cacheManager.IsExist(shortKey);
        }

        /// <summary>
        /// Generate base62 random string
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string RandomKey(int size = 7)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            var data = new byte[size];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            var result = new StringBuilder(size);

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            return result.ToString();
        }
    }
}
