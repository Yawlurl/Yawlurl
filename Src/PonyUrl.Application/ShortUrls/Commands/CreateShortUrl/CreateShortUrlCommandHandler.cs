using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Core;
using PonyUrl.Common;
using PonyUrl.Infrastructure.Redis;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, ShortUrlDto>
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IShortKeyManager _shortKeyManager;
        private readonly CreateShortUrlValidator validator;
        private readonly ICacheManager _cacheManager;

        public CreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository, IShortKeyManager shortKeyManager, ICacheManager cacheManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _shortKeyManager = shortKeyManager;
            _cacheManager = cacheManager;
            validator = new CreateShortUrlValidator();
        }

        public async Task<ShortUrlDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.Errors);
            }

            //Generate shortUrl
            var shortUrl = new ShortUrl(request.LongUrl)
            {
                ShortKey = await _shortKeyManager.GenerateShortKeyRandomAsync(cancellationToken)
            };

            //Save to database
            var result = await _shortUrlRepository.InsertAsync(shortUrl, cancellationToken);

            Check.ArgumentNotNullOrEmpty(result.Id);

            //Add to cache
            await _cacheManager.SetUrl(result.ShortKey, result.LongUrl);

            return new ShortUrlDto() { ShortKey = result.ShortKey, LongUrl = result.LongUrl };
        }
    }
}
