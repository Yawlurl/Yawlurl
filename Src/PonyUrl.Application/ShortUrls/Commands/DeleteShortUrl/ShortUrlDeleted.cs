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
    public class ShortUrlDeleted : INotification
    {
        public ShortUrl ShortUrl { get; set; }

        public class DeletedShortUrlHandler : INotificationHandler<ShortUrlDeleted>
        {
            public IShortUrlRepository _shortUrlRepository;

            public DeletedShortUrlHandler(IShortUrlRepository shortUrlRepository)
            {
                _shortUrlRepository = shortUrlRepository;
            }


            public async Task Handle(ShortUrlDeleted notification, CancellationToken cancellationToken)
            {
                //Delete from database
                if (!Check.IsGuidDefaultOrEmpty(notification.ShortUrl.Id))
                {
                    await _shortUrlRepository.DeleteAsync(notification.ShortUrl.Id, cancellationToken);
                }
            }
        }
    }
}
