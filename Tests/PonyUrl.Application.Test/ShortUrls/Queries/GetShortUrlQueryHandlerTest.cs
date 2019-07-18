using FluentAssertions;
using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Application.ShortUrls.Queries;
using PonyUrl.Core;
using PonyUrl.Domain;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PonyUrl.Application.Test.ShortUrls.Queries
{

    public class GetShortUrlQueryHandlerTest : BaseTest
    {
        private readonly CreateShortUrlCommandHandler _commandHandler;
        private readonly GetShortUrlQueryHandler _queryhandler;
        public GetShortUrlQueryHandlerTest()
        {
            _queryhandler = new GetShortUrlQueryHandler(That<IShortUrlRepository>());

            _commandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(),
                                                               That<IShortKeyManager>(),
                                                               That<ICacheManager>());
        }

        [Fact]
        public async Task GetShortUrl_Test()
        {
            var shortUrl = await _commandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.google.com"
            },
            CancellationToken.None);

            shortUrl.Should().NotBeNull();
            shortUrl.ShortKey.Should().NotBeNullOrEmpty();

            var result = await _queryhandler.Handle(new GetShortUrlQuery { ShortKey = shortUrl.ShortKey }, CancellationToken.None);

            result.Should().BeOfType<ShortUrlViewModel>();

            result.LongUrl.Should().Be("http://www.google.com");

        }
    }
}
