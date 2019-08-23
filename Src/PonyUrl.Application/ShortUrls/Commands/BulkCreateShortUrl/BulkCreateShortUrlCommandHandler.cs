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
        private readonly IMediator _mediator;

        public BulkCreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository,
                                                IShortKeyManager shortKeyManager,
                                                ICacheManager cacheManager,
                                                IMediator mediator)
        {
            _shortUrlRepository = shortUrlRepository;
            _shortKeyManager = shortKeyManager;
            _cacheManager = cacheManager;
            _mediator = mediator;
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

            //Add to cache
            foreach (var shortUrl in shortUrls)
            {
                await _cacheManager.SetUrl(shortUrl.ShortKey, shortUrl.LongUrl);

                result.Add(new ShortUrlDto() { ShortKey = shortUrl.ShortKey, LongUrl = shortUrl.LongUrl });
            }

            //Publish Event
            await _mediator.Publish(new ShortUrlsCreated { ShortUrls = shortUrls });

            return result;
        }
    }
}
