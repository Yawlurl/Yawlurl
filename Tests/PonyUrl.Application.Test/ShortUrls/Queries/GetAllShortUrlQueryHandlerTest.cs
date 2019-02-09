using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Application.ShortUrls.Queries;
using PonyUrl.Domain.Entities;
using PonyUrl.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Core;

namespace PonyUrl.Application.Test.ShortUrls.Queries
{
    public class GetAllShortUrlQueryHandlerTest : TestBase
    {
        private readonly GetAllShortUrlQueryHandler _queryhandler;
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public GetAllShortUrlQueryHandlerTest()
        {
            _commandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(), That<IShortKeyManager>());
            _queryhandler = new GetAllShortUrlQueryHandler(That<IShortUrlRepository>());
        }

        [Fact]
        public async Task GetAllShortUrl_Test()
        {
            var id = await _commandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.google.com"
            },
            CancellationToken.None);

            var result = await _queryhandler.Handle(new GetAllShortUrlQuery(), CancellationToken.None);

            result.Should().BeOfType<ShortUrlListViewModel>();

            result.ShortUrls.ToList().Count.Should().BeGreaterThan(0);
        }
    }
}
