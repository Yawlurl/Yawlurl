using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Application.ShortUrls.Queries;
using PonyUrl.Core;
using PonyUrl.Domain;
using PonyUrl.Infrastructure.AspNetCore.Models;
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
            _queryhandler = new GetShortUrlQueryHandler(ShortUrlRepositoryMock,
                                                        SlugManagerMock,
                                                        MediatorMock,
                                                        GlobalSettingsMock,
                                                        UserManagerMock,
                                                        HttpContextAccessorMock);

            _commandHandler = new CreateShortUrlCommandHandler(SlugManagerMock,
                                                               HttpContextAccessorMock,
                                                               MediatorMock,
                                                               GlobalSettingsMock,
                                                               ShortUrlRepositoryMock,
                                                               UserManagerMock);
        }

        [Fact]
        public async Task GetShortUrl_Test()
        {
            var shortUrl = await _commandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.abc.com"
            },
            CancellationToken.None);

            shortUrl.Should().NotBeNull();
            shortUrl.SlugKey.Should().NotBeNullOrEmpty();

            var result = await _queryhandler.Handle(new GetShortUrlQuery { SlugKey = shortUrl.SlugKey }, CancellationToken.None);

            result.Should().BeOfType<ShortUrlListDto>();

            result.LongUrl.Should().Be(shortUrl.LongUrl);

        }
    }
}
