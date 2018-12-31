using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain.Interfaces;
using PonyUrl.Domain.Entities;

namespace PonyUrl.Application.ShortUrls.Commands.CreateShortUrl
{
    public class CreateShortUrlCommandHandler :  IRequestHandler<CreateShortUrlCommand, Guid>
    {
        IShortUrlRepository _shortUrlRepository;
        public CreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository)
        {
            _shortUrlRepository = shortUrlRepository;
        }

        public async Task<Guid> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var shortUrl = new ShortUrl(request.LongUrl);

            // Generate Short Url code
            string code = "";// IShortKeyManager.GenerateShortKey(request.LongUrl);

            // TODO : ShortKey should be unique
            shortUrl.ShortKey = code;

            var result = await _shortUrlRepository.InsertAsync(shortUrl);
             
            return result.Id;
        }
    }
}
