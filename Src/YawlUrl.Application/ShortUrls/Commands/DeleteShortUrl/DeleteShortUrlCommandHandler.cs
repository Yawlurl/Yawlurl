using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YawlUrl.Domain;
using YawlUrl.Common;
using YawlUrl.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Infrastructure.AspNetCore.Models;

namespace YawlUrl.Application.ShortUrls.Commands
{
    public class DeleteShortUrlCommandHandler : BaseHandler<DeleteShortUrlCommand, bool>
    {
        #region Properties

        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IMediator _mediator;
        private readonly ISlugRepository _slugRepository;
        #endregion

        #region C'tor
        public DeleteShortUrlCommandHandler(IShortUrlRepository shortUrlRepository,
                                            IMediator mediator,
                                            IHttpContextAccessor httpContextAccessor,
                                            UserManager<ApplicationUser> userManager,
                                            ISlugRepository slugRepository)
            : base(httpContextAccessor, userManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _mediator = mediator;
            _slugRepository = slugRepository;
        }
        #endregion


        public override async Task<bool> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
        {
            //Get Slug
            var slug = await GetSlug(request.SlugKey, cancellationToken);

            //Check Permission
            CheckPermission(slug);

            //Get entity
            var shortUrl = await GetShortUrl(slug, cancellationToken);

            //Delete ShortUrl
            await DeleteShortUrl(shortUrl);

            //Publish Event and delete slug
            _mediator.Publish(new ShortUrlDeleted { ShortUrl = shortUrl }).Forget();

            return true;
        }

        #region Private Methods

        /// <summary>
        /// Only owners and admins can delete
        /// </summary>
        /// <param name="slug"></param>
        private void CheckPermission(Slug slug)
        {
            if (!CurrentUser.UserId.Equals(slug.CreatedBy) && !CurrentUser.IsAdmin())
            {
                throw new ApplicationException("Has no authority to delete this item");
            }
        }

        private async Task DeleteShortUrl(ShortUrl shortUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _shortUrlRepository.Delete(shortUrl.Id, cancellationToken);

            Check.That<ApplicationException>(!result, "Something went wrong.");
        }

        private async Task<Slug> GetSlug(string slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.That<ApplicationException>(Check.IsNullOrEmpty(slug), "Slug is null or empty");

            var slugEntity = await _slugRepository.GetByKey(slug);

            Check.That<DomainException>(Check.IsNull(slugEntity), "Slug is null or empty");

            return slugEntity;
        }

        private async Task<ShortUrl> GetShortUrl(Slug slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.That<ApplicationException>(Check.IsNull(slug), "Slug is null.");

            var shortUrl = await _shortUrlRepository.GetBySlug(slug.Id, cancellationToken);

            Check.That<DomainException>(Check.IsNull(shortUrl), "shortUrl is null.");

            return shortUrl;
        }



        #endregion
    }
}
