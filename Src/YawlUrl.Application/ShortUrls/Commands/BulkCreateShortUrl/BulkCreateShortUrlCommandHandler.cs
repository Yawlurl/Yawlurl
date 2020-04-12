using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Common;
using YawlUrl.Core;
using YawlUrl.Domain;
using YawlUrl.Infrastructure.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using YawlUrl.Domain.Core;

namespace YawlUrl.Application.ShortUrls.Commands.BulkCreateShortUrl
{
    public class BulkCreateShortUrlCommandHandler : BaseHandler<BulkCreateShortUrlCommand, List<ShortUrlDto>>
    {
        #region Fields
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly ISlugManager _slugManager;
        private readonly BulkCreateShortUrlValidator _validator;
        private readonly IMediator _mediator;
        private readonly IGlobalSettings _globalSettings;
        #endregion

        #region C'tor
        public BulkCreateShortUrlCommandHandler(IShortUrlRepository shortUrlRepository,
                                                ISlugManager slugManager,
                                                IMediator mediator,
                                                IGlobalSettings globalSettings,
                                                IHttpContextAccessor httpContextAccessor,
                                                UserManager<ApplicationUser> userManager)
            : base(httpContextAccessor, userManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _slugManager = slugManager;
            _mediator = mediator;
            _globalSettings = globalSettings;
        }
        #endregion

        #region Properties
        public override IValidator Validator { get { return _validator ?? new BulkCreateShortUrlValidator(); } }
        #endregion

        public override async Task<List<ShortUrlDto>> Handle(BulkCreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            List<Slug> slugs = new List<Slug>();
            List<ShortUrl> shortUrls = new List<ShortUrl>();
            Slug slug;
            foreach (var url in request.LongUrls)
            {
                //Generate shortUrl
                var shortUrl = new ShortUrl(url)
                {
                    CreatedBy = CurrentUser.UserId,
                    UpdatedBy = CurrentUser.UserId
                };
                slug = await _slugManager.Create(CurrentUser, string.Empty, true, cancellationToken);
                slugs.Add(slug);

                shortUrl.SetSlug(slug);

                shortUrls.Add(shortUrl);
            }

            //Add or Update to database
            await _shortUrlRepository.BulkInsert(shortUrls, cancellationToken);

            // Data
            var result = shortUrls.AsQueryable().Select(s => ShortUrlDto.MapFromEntity(s, _globalSettings.RouterDomain)).ToList();

            //Publish Event
            _mediator.Publish(new ShortUrlsCreated { ShortUrls = shortUrls }).Forget();

            return result;
        }
    }
}
