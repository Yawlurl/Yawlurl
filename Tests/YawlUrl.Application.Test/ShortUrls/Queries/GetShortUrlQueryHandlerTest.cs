using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Application.ShortUrls.Commands;
using YawlUrl.Application.ShortUrls.Queries;
using YawlUrl.Core;
using YawlUrl.Domain;
using YawlUrl.Infrastructure.AspNetCore.Models;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace YawlUrl.Application.Test.ShortUrls.Queries
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
