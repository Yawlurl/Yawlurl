using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using FluentAssertions;
using System;
using PonyUrl.Core;

namespace PonyUrl.Application.Test.ShortUrls.Commands
{
    public class CreateShortUrlCommandHandlerTest : BaseTest
    {
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public CreateShortUrlCommandHandlerTest()
        {
            _commandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(), That<IShortKeyManager>(), That<ICacheManager>());
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
