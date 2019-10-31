using MediatR;
using YawlUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YawlUrl.Application
{
    public class ShortUrlsQueried: INotification
    {
        public List<ShortUrl> ShortUrls { get; set; }

        public class ShortUrlsQueriedHandler : INotificationHandler<ShortUrlsQueried>
        {
            public async Task Handle(ShortUrlsQueried notification, CancellationToken cancellationToken)
            {
               // TODO: Log
            }
        }
    }
}
