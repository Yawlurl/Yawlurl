using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Common;
using PonyUrl.Core;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class DeleteShortUrlCommandHandler : IRequestHandler<DeleteShortUrlCommand>
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;

        public DeleteShortUrlCommandHandler(IShortUrlRepository shortUrlRepository, ICacheManager cacheManager, IMediator mediator)
        {
            _shortUrlRepository = shortUrlRepository;
            _cacheManager = cacheManager;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
        {
            Check.ArgumentNotNullOrEmpty(request.ShortKey);
            
            //Delete from cache
            if (await _cacheManager.DeleteUrl(request.ShortKey))
            {
                //Get entity
                var shortUrl = await _shortUrlRepository.GetByShortKeyAsync(request.ShortKey);

                await _mediator.Publish(new ShortUrlDeleted { ShortUrl = shortUrl });
            }

            return Unit.Value;
        }
    }
}
