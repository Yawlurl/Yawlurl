using MediatR;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using PonyUrl.Common;

namespace PonyUrl.Application
{
    public class ShortUrlsCreated : INotification
    {
        public List<ShortUrl> ShortUrls { get; set; }

        public class ShortUrlsCreatedHandler : INotificationHandler<ShortUrlsCreated>
        {
            private readonly ISlugRepository _slugRepository;

            public ShortUrlsCreatedHandler(ISlugRepository slugRepository)
            {
                _slugRepository = slugRepository;
            }

            public async Task Handle(ShortUrlsCreated notification, CancellationToken cancellationToken)
            {
                if (notification.ShortUrls.Any(s => !Check.IsGuidDefaultOrEmpty(s.Id)))
                {
                    foreach (var shortUrl in notification.ShortUrls.Where(s => !Check.IsGuidDefaultOrEmpty(s.Id)))
                    {
                        var slug = await _slugRepository.Get(shortUrl.SlugId);

                        //Activate ShortKey
                        slug.Activate();

                        slug.UpdatedBy = shortUrl.UpdatedBy;
                        slug.UpdatedDate = shortUrl.UpdatedDate;

                        //Update
                        await _slugRepository.Update(slug, cancellationToken);
                    }
                }
            }
        }
    }
}
