using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YawlUrl.Domain;
using YawlUrl.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Infrastructure.AspNetCore.Models;
using YawlUrl.Common;
using System.Linq;
using FluentValidation;
using YawlUrl.Domain.Core;

namespace YawlUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlCommandHandler : BaseHandler<CreateShortUrlCommand, ShortUrlDto>
    {
        #region Fields
        private readonly ISlugManager _slugManager;
        private readonly CreateShortUrlValidator _validator;
        private readonly IMediator _mediator;
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IGlobalSettings _globalSettings;
        #endregion

        #region C'tor
        public CreateShortUrlCommandHandler(ISlugManager slugManager,
                                            IHttpContextAccessor httpContextAccessor,
                                            IMediator mediator,
                                            IGlobalSettings globalSettings,
                                            IShortUrlRepository shortUrlRepository,
                                            UserManager<ApplicationUser> userManager)
            : base(httpContextAccessor, userManager)
        {
            _slugManager = slugManager;
            _mediator = mediator;
            _shortUrlRepository = shortUrlRepository;
            _globalSettings = globalSettings;

        }
        #endregion

        #region Properties
        public override IValidator Validator
        {
            get
            {
                return _validator ?? new CreateShortUrlValidator();
            }
        }
        #endregion

        public override async Task<ShortUrlDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);

            if (request.IsRouter)
            {
                CurrentUser = AnonymousUser.Current();

                // Only check url dublication for anonymous user
                var existUrl = await _shortUrlRepository.GetUserShortUrlByLongUrl(request.LongUrl, CurrentUser.UserId, cancellationToken);

                if (Check.IsNotNull(existUrl))
                {
                    return ShortUrlDto.MapFromEntity(existUrl, _globalSettings.RouterDomain);
                }
            }

            //Generate shortUrl
            var shortUrl = new ShortUrl(request.LongUrl)
            {
                CreatedBy = CurrentUser.UserId,
                UpdatedBy = CurrentUser.UserId
            };

            var slug = await CreateSlug(request.SlugKey);

            shortUrl.SetSlug(slug);

            //Save to database
            await _shortUrlRepository.InsertOrUpdate(shortUrl, cancellationToken);

            //Created Event
            _mediator.Publish(new ShortUrlCreated { ShortUrl = shortUrl }, cancellationToken).Forget();

            return ShortUrlDto.MapFromEntity(shortUrl, _globalSettings.RouterDomain); ;
        }

        #region Private Methods
        private async Task<Slug> CreateSlug(string slug, CancellationToken cancellationToken = default)
        {
            var slugEntity = Check.IsNullOrEmpty(slug) ? await _slugManager.Create(CurrentUser, string.Empty, true, cancellationToken)
                                                       : await _slugManager.Create(CurrentUser, slug, false, cancellationToken);

            Check.That<ApplicationException>(Check.IsNull(slugEntity), "Something went wrong while the slug creating.");

            return slugEntity;
        }
        #endregion
    }
}
