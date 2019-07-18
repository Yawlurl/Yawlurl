using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Domain;
using System.Threading.Tasks;
using Xunit;
using System;
using FluentAssertions;
using System.Threading;
using PonyUrl.Core;

namespace PonyUrl.Application.Test.ShortUrls.Commands
{
    public class DeleteShortUrlCommandHandlerTest : BaseTest
    {
        private readonly CreateShortUrlCommandHandler _createCommandHandler;
        private readonly DeleteShortUrlCommandHandler _deleteCommandHandler;


        public DeleteShortUrlCommandHandlerTest()
        {
            _createCommandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(),
                                                                     That<IShortKeyManager>(),
                                                                     That<ICacheManager>());

            _deleteCommandHandler = new DeleteShortUrlCommandHandler(That<IShortUrlRepository>(), 
                                                                     That<ICacheManager>());
        }


        [Fact]
        public async Task DeleteShortUrl()
        {
            var shortUrl = await _createCommandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.google.com"
            },
            CancellationToken.None);

            shortUrl.Should().NotBeNull();
            shortUrl.ShortKey.Should().NotBeNullOrEmpty();

            await _deleteCommandHandler.Handle(new DeleteShortUrlCommand() { ShortKey = shortUrl.ShortKey }, CancellationToken.None);

            var entity = await That<IShortUrlRepository>().GetByShortKeyAsync(shortUrl.ShortKey);

            entity.Should().BeNull("Entity should be null!");
        }



    }
}
