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
using MediatR;

namespace PonyUrl.Application.Test.ShortUrls.Queries
{
    public class GetAllShortUrlQueryHandlerTest : BaseTest
    {
        private readonly GetAllShortUrlQueryHandler _queryhandler;
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public GetAllShortUrlQueryHandlerTest()
        {
            _commandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(), 
                                                               That<IShortKeyManager>(),
                                                               That<ICacheManager>(),
                                                               That<IMediator>());

            _queryhandler = new GetAllShortUrlQueryHandler(That<IShortUrlRepository>(), That<ICacheManager>());
        }

        [Fact]
        public async Task GetAllShortUrl_Test()
        {
            var shortUrl = await _commandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.google.com"
            },
            CancellationToken.None);

            shortUrl.Should().NotBeNull();
            shortUrl.ShortKey.Should().NotBeNullOrEmpty();

            var result = await _queryhandler.Handle(new GetAllShortUrlQuery(), CancellationToken.None);

            result.Should().BeOfType<ShortUrlListViewModel>();

            result.ShortUrls.ToList().Count.Should().BeGreaterThan(0);
        }
    }
}
