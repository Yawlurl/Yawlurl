using FluentAssertions;
using YawlUrl.Application.ShortUrls.Queries;
using YawlUrl.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using YawlUrl.Application.ShortUrls.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YawlUrl.Infrastructure.AspNetCore.Models;

namespace YawlUrl.Application.Test.ShortUrls.Queries
{
    public class GetAllShortUrlQueryHandlerTest : BaseTest
    {
        private readonly GetAllShortUrlQueryHandler _queryhandler;
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public GetAllShortUrlQueryHandlerTest()
        {
            _commandHandler = new CreateShortUrlCommandHandler(SlugManagerMock,
                                                               HttpContextAccessorMock,
                                                               MediatorMock,
                                                               GlobalSettingsMock,
                                                               ShortUrlRepositoryMock,
                                                               UserManagerMock);

            _queryhandler = new GetAllShortUrlQueryHandler(ShortUrlRepositoryMock,
                                                           UserManagerMock,
                                                           GlobalSettingsMock,
                                                           HttpContextAccessorMock, 
                                                           MediatorMock);
        }

        [Fact]
        public async Task GetAllShortUrl_Test()
        {
            var shortUrl = await _commandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.abc.com"
            },
            CancellationToken.None);

            shortUrl.Should().NotBeNull();
            shortUrl.SlugKey.Should().NotBeNullOrEmpty();

            var result = await _queryhandler.Handle(new GetAllShortUrlQuery(), CancellationToken.None);

            result.Should().BeOfType<ShortUrlListDto>();

            result.ShortUrls.ToList().Count.Should().BeGreaterThan(0);
        }
    }
}
