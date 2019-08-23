using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Common;
using PonyUrl.Core;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetShortUrlQueryHandler : IRequestHandler<GetShortUrlQuery, ShortUrlViewModel>
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;

        public GetShortUrlQueryHandler(IShortUrlRepository shortUrlRepository, ICacheManager cacheManager, IMediator mediator)
        {
            _shortUrlRepository = shortUrlRepository;
            _cacheManager = cacheManager;
            _mediator = mediator;
        }

        public async Task<ShortUrlViewModel> Handle(GetShortUrlQuery request, CancellationToken cancellationToken)
        {
            var result = new ShortUrlViewModel();

            //Check from cache
            var shortUrl = await _cacheManager.GetUrl(request.ShortKey);

            if (Check.IsNullOrEmpty(shortUrl))
            {
                //Check from db
                var entity = await _shortUrlRepository.GetByShortKeyAsync(request.ShortKey);

                Check.That<ApplicationException>(Check.IsNull(entity), "ShortUrl cannot find!");

                //Set Hits
                entity.Boost();
                await _shortUrlRepository.UpdateAsync(entity, cancellationToken);

                //Set cache
                await _cacheManager.SetUrl(entity.ShortKey, entity.LongUrl);

                //Map
                result.MapFromEntity(entity);
            }
            else
            {
                //Check from db
                var isExist = await _shortUrlRepository.IsExistAsync(request.ShortKey, cancellationToken);

                if (!isExist)
                {
                    //Delete from cache
                    await _cacheManager.DeleteUrl(request.ShortKey);

                    throw new ApplicationException($"This shortKey:'{request.ShortKey}' not found");
                }
                else
                {
                    //Get Entity
                    var entity = await _shortUrlRepository.GetByShortKeyAsync(request.ShortKey, cancellationToken);
                    
                    //Set Hits
                    entity.Boost();
                    
                    //Update Entity
                    await _shortUrlRepository.UpdateAsync(entity, cancellationToken);

                    //Map
                    result.MapFromEntity(entity);
                }
            }

            return result;

        }
    }
}
