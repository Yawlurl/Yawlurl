using MediatR;
using YawlUrl.Common;
using YawlUrl.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Application.ShortUrls.Commands
{
    public class ShortUrlDeleted : INotification
    {
        public ShortUrl ShortUrl { get; set; }

        public class DeletedShortUrlHandler : INotificationHandler<ShortUrlDeleted>
        {
            public ISlugRepository _slugRepository;

            public DeletedShortUrlHandler(ISlugRepository slugRepository)
            {
                _slugRepository = slugRepository;
            }


            public async Task Handle(ShortUrlDeleted notification, CancellationToken cancellationToken)
            {
                //TODO:Log

                //Deactivate Slug
                await DeactiveSlug(notification.ShortUrl.SlugId);

            }


            private async Task DeactiveSlug(Guid slugId)
            {
                var slug = await _slugRepository.Get(slugId);

                if (Check.IsNotNull(slug))
                {
                    slug.DeActivate();

                    await _slugRepository.Update(slug);
                }
            }
        }
    }
}
