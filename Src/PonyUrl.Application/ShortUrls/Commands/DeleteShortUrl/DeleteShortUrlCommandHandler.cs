using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Common;
using PonyUrl.Core;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class DeleteShortUrlCommandHandler :   IRequestHandler<DeleteShortUrlCommand>
    {
        IShortUrlRepository _shortUrlRepository;
        ICacheManager _cacheManager;
        public DeleteShortUrlCommandHandler(IShortUrlRepository shortUrlRepository,ICacheManager cacheManager)  
        {
            _shortUrlRepository = shortUrlRepository;
            _cacheManager = cacheManager;
        }

        public async Task<Unit> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
        { 
            Check.ArgumentNotNullOrEmpty(request.ShortKey);
            
            //Get entity
            var shortUrl = await _shortUrlRepository.GetByShortKeyAsync(request.ShortKey);
            
            //Delete from db
            var deleteResult = await _shortUrlRepository.DeleteAsync(shortUrl.Id);
             
            //Delete from cache
            if(deleteResult)
            {
                await _cacheManager.DeleteUrl(request.ShortKey);
            }

            return Unit.Value;
        }
    }
}
