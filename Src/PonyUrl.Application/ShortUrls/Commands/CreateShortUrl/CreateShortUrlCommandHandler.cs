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
        private readonly IMediator _mediator;
        public CreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository, 
                                            IShortKeyManager shortKeyManager, 
                                            ICacheManager cacheManager, 
                                            IMediator mediator)
        {
            _shortUrlRepository = shortUrlRepository;
            _shortKeyManager = shortKeyManager;
            _cacheManager = cacheManager;
            _mediator = mediator;
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

            //Add to cache
            await _cacheManager.SetUrl(shortUrl.ShortKey, shortUrl.LongUrl);

            //Created Event
            await _mediator.Publish(new ShortUrlCreated { ShortUrl = shortUrl }, cancellationToken);

            return new ShortUrlDto() { ShortKey = shortUrl.ShortKey, LongUrl = shortUrl.LongUrl };
        }
    }
}
