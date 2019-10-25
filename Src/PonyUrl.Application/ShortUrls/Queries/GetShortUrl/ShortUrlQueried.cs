using MediatR;
using PonyUrl.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PonyUrl.Application
{
    public class ShortUrlQueried : INotification
    {
        public ShortUrl ShortUrl { get; set; }

        public class ShortUrlQueriedHandler : INotificationHandler<ShortUrlQueried>
        {
            public async Task Handle(ShortUrlQueried notification, CancellationToken cancellationToken)
            {
                //TODO: Log or stat
            }
        }
    }
}
