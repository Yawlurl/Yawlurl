using PonyUrl.Application.ShortUrls.Commands.CreateShortUrl;
using PonyUrl.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using FluentAssertions;
using System;

namespace PonyUrl.Application.Test.ShortUrls.Commands
{
    public class CreateShortUrlCommandHandlerTest : TestBase
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public CreateShortUrlCommandHandlerTest()
        {
           
            var serviceProvider = services.BuildServiceProvider();

            _shortUrlRepository = serviceProvider.GetService<IShortUrlRepository>();

            _commandHandler = new CreateShortUrlCommandHandler(_shortUrlRepository);
        }

        [Fact]
        public async Task CreateShortUrl_Test()
        {
            var command = new CreateShortUrlCommand
            {
                LongUrl = "http://www.google.com"
            };
             
            var result = await _commandHandler.Handle(command, CancellationToken.None);

            result.Should<Guid>();
        }
    }
}
