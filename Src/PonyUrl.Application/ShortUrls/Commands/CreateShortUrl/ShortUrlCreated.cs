using MediatR;
using PonyUrl.Common;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class ShortUrlCreated : INotification
    {
        public ShortUrl ShortUrl { get; set; }

        public class ShortUrlCreatedHandler : INotificationHandler<ShortUrlCreated>
        {
            private readonly IShortUrlRepository _shortUrlRepository;

            public ShortUrlCreatedHandler(IShortUrlRepository shortUrlRepository)
            {
                _shortUrlRepository = shortUrlRepository;
            }

            public async Task Handle(ShortUrlCreated notification, CancellationToken cancellationToken)
            {
                //Save to database
                if(Check.IsGuidDefaultOrEmpty(notification.ShortUrl.Id))
                {
                    await _shortUrlRepository.InsertAsync(notification.ShortUrl, cancellationToken);
                }
            }
        }
    }
}
