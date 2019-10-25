using MediatR;
using Microsoft.Extensions.Logging;
using PonyUrl.Common;
using PonyUrl.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class ShortUrlCreated : INotification
    {
        public ShortUrl ShortUrl { get; set; }


        public class ShortUrlCreatedHandler : INotificationHandler<ShortUrlCreated>
        {
            private readonly ILogger<ShortUrlCreated> _logger;
            private readonly ISlugRepository _slugRepository;

            public ShortUrlCreatedHandler(ILogger<ShortUrlCreated> logger, ISlugRepository slugRepository)
            {
                _logger = logger;
                _slugRepository = slugRepository;
            }

            public async Task Handle(ShortUrlCreated notification, CancellationToken cancellationToken)
            {
                //TODO: Log
                if (!Check.IsGuidDefaultOrEmpty(notification.ShortUrl.Id))
                {
                    var slug = await _slugRepository.Get(notification.ShortUrl.SlugId);

                    //Activate slug
                    slug.UpdatedBy = notification.ShortUrl.UpdatedBy;
                    slug.UpdatedDate = notification.ShortUrl.UpdatedDate;

                    if (!Check.IsGuidDefaultOrEmpty(notification.ShortUrl.Id))
                    {
                        slug.Activate();
                    }

                    await _slugRepository.Update(slug, cancellationToken);
                }
            }
        }
    }
}
