using MediatR;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using PonyUrl.Common;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class ShortUrlsCreated : INotification
    {
        public List<ShortUrl> ShortUrls { get; set; }

        public class ShortUrlsCreatedHandler : INotificationHandler<ShortUrlsCreated>
        {
            private readonly IShortUrlRepository _shortUrlRepository;

            public ShortUrlsCreatedHandler(IShortUrlRepository shortUrlRepository)
            {
                _shortUrlRepository = shortUrlRepository;
            }

            public async Task Handle(ShortUrlsCreated notification, CancellationToken cancellationToken)
            {
                //Save to database
                if (notification.ShortUrls.TrueForAll(s => Check.IsGuidDefaultOrEmpty(s.Id)))
                {
                    await _shortUrlRepository.BulkInsertAsync(notification.ShortUrls, cancellationToken);
                }
            }
        }
    }
}
