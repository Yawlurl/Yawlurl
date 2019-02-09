﻿using PonyUrl.Application.ShortUrls.Commands;
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
    public class CreateShortUrlCommandHandlerTest : TestBase
    {
        private readonly CreateShortUrlCommandHandler _commandHandler;

        public CreateShortUrlCommandHandlerTest()
        {
            _commandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(), That<IShortKeyManager>());
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
