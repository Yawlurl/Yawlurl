using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain;
using PonyUrl.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PonyUrl.Infrastructure.AspNetCore.Models;
using System;
using PonyUrl.Domain.Core;

namespace PonyUrl.Application.ShortUrls.Queries
{
    public class GetShortUrlQueryHandler : BaseHandler<GetShortUrlQuery, ShortUrlDto>
    {
        #region Fields
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly ISlugManager _slugManager;
        private readonly IMediator _mediator;
        private readonly IGlobalSettings _globalSetting;
        #endregion

        #region C'tor
        public GetShortUrlQueryHandler(IShortUrlRepository shortUrlRepository,
                                       ISlugManager slugManager,
                                       IMediator mediator,
                                       IGlobalSettings globalSettings,
                                       UserManager<ApplicationUser> userManager,
                                       IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, userManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _slugManager = slugManager;
            _mediator = mediator;
            _globalSetting = globalSettings;

        }
        #endregion


        public override async Task<ShortUrlDto> Handle(GetShortUrlQuery request, CancellationToken cancellationToken)
        {
            var data = new ShortUrlDto();

            // Get SlugId
            var slugId = await CheckAndGetSlugId(request.SlugKey, cancellationToken);

            // Get ShortUrl
            var shortUrlEntity = await CheckAndGetShortUrl(slugId, request.Boost, cancellationToken);

            // Map
            data.MapFromEntity(shortUrlEntity, _globalSetting.RouterDomain);

            // @Event
            await _mediator.Publish(new ShortUrlQueried() { ShortUrl = shortUrlEntity }, cancellationToken);

            return data;
        }

        #region Methods

        private async Task<Guid> CheckAndGetSlugId(string keyword, CancellationToken cancellationToken)
        {
            var slugId = await _slugManager.GetSlugIdByKeyword(keyword, cancellationToken);

            Check.That<ApplicationException>(Check.IsGuidDefaultOrEmpty(slugId), $"This slug:'{keyword}' not found.");

            var isExist = await _shortUrlRepository.IsExistBySlug(slugId, cancellationToken);

            //Check existence
            Check.That<ApplicationException>(!isExist, $"This slug:'{keyword}' not found");

            return slugId;
        }

        private async Task<ShortUrl> CheckAndGetShortUrl(Guid slugId, bool boost, CancellationToken cancellationToken)
        {
            //Get shorturl entity
            var shortUrlEntity = await _shortUrlRepository.GetBySlug(slugId, cancellationToken);

            //Check owner
            Check.That<ApplicationException>(!CurrentUser.IsAdmin() &&
                !shortUrlEntity.CreatedBy.Equals(CurrentUser.Id), $"This slug not found for the user");

            //Set Hits
            if (boost)
            {
                shortUrlEntity.Boost();
            }

            //Update the shorturl
            await _shortUrlRepository.Update(shortUrlEntity, cancellationToken);

            return shortUrlEntity;
        }



        #endregion

    }
}
