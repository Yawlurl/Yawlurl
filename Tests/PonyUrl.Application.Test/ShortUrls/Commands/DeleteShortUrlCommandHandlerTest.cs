using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Domain;
using System.Threading.Tasks;
using Xunit;
using System;
using FluentAssertions;
using System.Threading;
using PonyUrl.Core;

namespace PonyUrl.Application.Test.ShortUrls.Commands
{
    public class DeleteShortUrlCommandHandlerTest : TestBase
    {
        private readonly CreateShortUrlCommandHandler _createCommandHandler;
        private readonly DeleteShortUrlCommandHandler _deleteCommandHandler;


        public DeleteShortUrlCommandHandlerTest()
        {
            _createCommandHandler = new CreateShortUrlCommandHandler(That<IShortUrlRepository>(), 
                                                                     That<IShortKeyManager>());
            _deleteCommandHandler = new DeleteShortUrlCommandHandler(That<IShortUrlRepository>());
        }


        [Fact]
        public async Task DeleteShortUrl()
        {
            var id = await _createCommandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.google.com"
            },
            CancellationToken.None);

            id.Should().As<Guid>();

            await _deleteCommandHandler.Handle(new DeleteShortUrlCommand() { Id = id }, CancellationToken.None);

            var entity = await That<IShortUrlRepository>().GetAsync(id);

            entity.Should().BeNull("Entity should be null!");
        }



    }
}
