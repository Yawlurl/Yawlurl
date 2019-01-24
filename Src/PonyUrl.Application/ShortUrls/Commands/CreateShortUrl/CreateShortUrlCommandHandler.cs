using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Core;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, Guid>
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IShortKeyManager _shortKeyManager;
        private readonly CreateShortUrlValidator validator;

        public CreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository, IShortKeyManager shortKeyManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _shortKeyManager = shortKeyManager;
            validator = new CreateShortUrlValidator();
        }

        public async Task<Guid> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);

            if(!validationResult.IsValid)
            {
                throw new ApplicationException(validationResult.Errors);
            }

            var shortUrl = new ShortUrl(request.LongUrl)
            {
                ShortKey = await _shortKeyManager.GenerateShortKeyRandomAsync(cancellationToken)
            };

            var result = await _shortUrlRepository.InsertAsync(shortUrl, cancellationToken);

            return result.Id;
        }
    }
}
