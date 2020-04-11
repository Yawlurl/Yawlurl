using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YawlUrl.Domain;
using YawlUrl.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Infrastructure.AspNetCore.Models;
using YawlUrl.Common;
using YawlUrl.Domain.Core;

namespace YawlUrl.Application.ShortUrls.Queries
{
    public class GetAllShortUrlQueryHandler : BaseHandler<GetAllShortUrlQuery, ShortUrlListDto>
    {
        #region Fields
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IMediator _mediator;
        private readonly IGlobalSettings _globalSettings;
        #endregion

        #region C'tor
        public GetAllShortUrlQueryHandler(IShortUrlRepository shortUrlRepository,
                                          UserManager<ApplicationUser> userManager,
                                          IGlobalSettings globalSettings,
                                          IHttpContextAccessor httpContextAccessor,
                                          IMediator mediator)
            : base(httpContextAccessor, userManager)
        {
            _shortUrlRepository = shortUrlRepository;
            _mediator = mediator;
            _globalSettings = globalSettings;
        }
        #endregion

        public override async Task<ShortUrlListDto> Handle(GetAllShortUrlQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var totalCount = await GetTotalCount(cancellationToken);

            var result = new ShortUrlListDto
            {
                TotalCount = totalCount
            };

            List<ShortUrl> list = await GetShortUrls(request, totalCount);

            if (list.Any())
                result.ShortUrls = list.AsQueryable().Select(s => ShortUrlDto.MapFromEntity(s, _globalSettings.RouterDomain));

            // @Event
            await _mediator.Publish(new ShortUrlsQueried() { ShortUrls = list }, cancellationToken);

            return result;
        }

        #region Methods
        private async Task<long> GetTotalCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            return CurrentUser.IsAdmin() ? await _shortUrlRepository.Count(cancellationToken)
                                         : await _shortUrlRepository.GetCountByUser(CurrentUser.Id, cancellationToken);
        }

        private async Task<List<ShortUrl>> GetShortUrls(GetAllShortUrlQuery request, long totalCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ShortUrl> list = new List<ShortUrl>();

            if (totalCount > 0 && request.Index.HasValue && request.Limit.HasValue && request.Limit.Value > 0)
            {
                list = CurrentUser.IsAdmin() ? await _shortUrlRepository.GetAllPagination(request.Index.Value, request.Limit.Value, cancellationToken)
                                             : await _shortUrlRepository.GetAllPaginationShortUrlsByUser(request.Index.Value, request.Limit.Value, CurrentUser.Id, cancellationToken);
            }
            else if (totalCount > 0)
            {
                list = CurrentUser.IsAdmin() ? await _shortUrlRepository.GetAll(cancellationToken)
                                             : await _shortUrlRepository.GetAllShortUrlsByUser(CurrentUser.Id, cancellationToken);
            }

            return list;
        }
        #endregion
    }
}
