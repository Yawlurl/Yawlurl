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

    public class GetShortUrlQueryHandlerTest : TestBase
    {
        private readonly CreateShortUrlCommandHandler _commandHandler;
        private readonly GetShortUrlQueryHandler _queryhandler;
        public GetShortUrlQueryHandlerTest()
        {
            _queryhandler = new GetShortUrlQueryHandler(That<IShortUrlRepository>());
            _commandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(), That<IShortKeyManager>());
        }

        [Fact]
        public async Task GetShortUrl_Test()
        {
            var id = await _commandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.google.com"
            },
            CancellationToken.None);

            var result = await _queryhandler.Handle(new GetShortUrlQuery { Id = id }, CancellationToken.None);

            result.Should().BeOfType<ShortUrlViewModel>();

            result.LongUrl.Should().Be("http://www.google.com");

        }
    }
}
