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
    /// Keyword generating management
    /// </summary>
    public class SlugManager : ISlugManager
    {
        #region Fields
        private readonly ISlugRepository _slugRepository;
        private readonly IShortUrlRepository _shortUrlRepository;
        #endregion

        public SlugManager(ISlugRepository slugRepository, IShortUrlRepository shortUrlRepository)
        {
            _slugRepository = slugRepository;
            _shortUrlRepository = shortUrlRepository;

        }

        #region Methods

        /// <summary>
        /// Generate unique shortkey
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Slug> Create(IUser user, string keyword = "", bool isRandom = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            Slug slug;

            if (isRandom)
            {
                bool isExist;
                do
                {
                    slug = new Slug(RandomKey());

                    isExist = await IsExist(slug.Key);

                } while (isExist);
            }
            else
            {
                slug = new Slug(keyword, false);

                if (await IsExist(slug.Key, cancellationToken))
                {
                    throw new DomainException($"This '{slug.Key}' key is already exist.");
                }
            }

            //Set Owner
            slug.SetOwner(user);

            //Create ShortKey Db
            await _slugRepository.Insert(slug, cancellationToken);

            Check.That<ArgumentNullException>(Check.IsGuidDefaultOrEmpty(slug.Id), nameof(slug.Id));

            return slug;
        }

        public async Task<bool> Activate(string keyword, CancellationToken cancellationToken = default)
        {
            var slug = await GetSlugByKeyword(keyword, cancellationToken);

            slug.DeActivate();

            return (await _slugRepository.Update(slug)).IsActive;
        }

        public async Task<bool> Deactivate(string keyword, CancellationToken cancellationToken = default)
        {
            var slug = await GetSlugByKeyword(keyword, cancellationToken);

            slug.DeActivate();

            return !(await _slugRepository.Update(slug)).IsActive;
        }

        public async Task<Slug> GetSlugByKeyword(string keyword, CancellationToken cancellationToken = default)
        {
            Check.ArgumentNotNullOrEmpty(keyword);

            return await _slugRepository.GetByKey(keyword, cancellationToken);
        }

        public async Task<Guid> GetSlugIdByKeyword(string keyword, CancellationToken cancellationToken = default)
        {
            return (await GetSlugByKeyword(keyword, cancellationToken)).Id;
        }

        /// <summary>
        /// Check key if exist in the collection
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsExist(string keyword, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _slugRepository.IsExistByKey(keyword, cancellationToken);
        }


        public async Task<bool> IsAssociated(Slug slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.ArgumentNotNull(slug);

            return await _shortUrlRepository.IsExistBySlug(slug.Id, cancellationToken);
        }

        /// <summary>
        /// Generate base62 random string
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string RandomKey(int size = Constants.MinSize)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            size = Math.Max(size, Constants.MinSize) - 1;

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
            //P is sign
            return string.Concat(Constants.RandomPrefix, result.ToString());
        }

        #endregion
    }


}
