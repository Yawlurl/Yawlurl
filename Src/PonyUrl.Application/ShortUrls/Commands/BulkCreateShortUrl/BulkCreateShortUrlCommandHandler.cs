using MediatR;
using PonyUrl.Common;
using PonyUrl.Core;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Application.ShortUrls.Commands.BulkCreateShortUrl
{
    public class BulkCreateShortUrlCommandHandler : IRequestHandler<BulkCreateShortUrlCommand, List<ShortUrlDto>>
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IShortKeyManager _shortKeyManager;
        private readonly BulkCreateShortUrlValidator validator;
        private readonly ICacheManager _cacheManager;

        public BulkCreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository, 
                                                IShortKeyManager shortKeyManager, 
                                                ICacheManager cacheManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _shortKeyManager = shortKeyManager;
            _cacheManager = cacheManager;
            validator = new BulkCreateShortUrlValidator();
        }


        public async Task<List<ShortUrlDto>> Handle(BulkCreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.Errors);
            }

            var result = new List<ShortUrlDto>();

            List<ShortUrl> shortUrls = new List<ShortUrl>();

            foreach (var url in request.LongUrls)
            {
                //Generate shortUrl
                var shortUrl = new ShortUrl(url)
                {
                    ShortKey = await _shortKeyManager.GenerateShortKeyRandomAsync(cancellationToken)
                };

                shortUrls.Add(shortUrl);
            }

            //Save to database
            var savedShortUrls = await _shortUrlRepository.BulkInsertAsync(shortUrls, cancellationToken);

            //Add to cache
            foreach (var shortUrl in savedShortUrls)
            {
                
                await _cacheManager.SetUrl(shortUrl.ShortKey, shortUrl.LongUrl);

                Check.ArgumentNotNullOrEmpty(shortUrl.Id);

                result.Add(new ShortUrlDto() { ShortKey = shortUrl.ShortKey, LongUrl = shortUrl.LongUrl });
            }
           
            return result;
        }
    }
}
