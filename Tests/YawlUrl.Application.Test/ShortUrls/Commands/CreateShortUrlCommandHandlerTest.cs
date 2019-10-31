using YawlUrl.Application.ShortUrls.Commands;
using YawlUrl.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using FluentAssertions;
using System;
using YawlUrl.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using YawlUrl.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Identity;

namespace YawlUrl.Application.Test.ShortUrls.Commands
{
    public class CreateShortUrlCommandHandlerTest : BaseTest
    {
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public CreateShortUrlCommandHandlerTest()
        {
            _commandHandler = new CreateShortUrlCommandHandler(SlugManagerMock,
                                                               HttpContextAccessorMock,
                                                               MediatorMock,
                                                               GlobalSettingsMock,
                                                               ShortUrlRepositoryMock,
                                                               UserManagerMock);
        }

        [Fact]
        public async Task CreateShortUrl_Test()
        {
            var command = new CreateShortUrlCommand
            {
                LongUrl = "http://www.google.com"
            };

            var result = await _commandHandler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();

        }

        [Fact]
        public async Task CreateShortUrl_InvalidFormat_Test()
        {
            var command = new CreateShortUrlCommand
            {
                LongUrl = "www.google.com"
            };

            await Assert.ThrowsAsync<ApplicationException>(() => _commandHandler.Handle(command, CancellationToken.None));
        }


    }
}
