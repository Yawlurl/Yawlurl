using PonyUrl.Application.ShortUrls.Commands;
using PonyUrl.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using PonyUrl.Domain.Entities;
using System;
using System.Linq;
using FluentAssertions;
using System.Threading;

namespace PonyUrl.Application.Test.ShortUrls.Commands
{
   public  class DeleteShortUrlCommandHandlerTest : TestBase
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly DeleteShortUrlCommandHandler _commandHandler;
         

        public DeleteShortUrlCommandHandlerTest()
        {

            var serviceProvider = services.BuildServiceProvider();

            _shortUrlRepository = serviceProvider.GetService<IShortUrlRepository>();

            _shortUrlRepository.InsertAsync(new ShortUrl("http://www.google.com"));
            _shortUrlRepository.InsertAsync(new ShortUrl("http://www.yahoo.com"));

            _commandHandler = new DeleteShortUrlCommandHandler(_shortUrlRepository);
        }


        [Fact]
        public async Task DeleteShortUrl()
        {
             
            var list = await _shortUrlRepository.GetAllAsync();
            var id = list.Find(q => q.LongUrl == "http://www.google.com").Id;

            id.Should().As<Guid>();

            DeleteShortUrlCommand command = new DeleteShortUrlCommand
            {
                Id = id
            };

            await _commandHandler.Handle(command, CancellationToken.None);

            var entity = await _shortUrlRepository.GetAsync(id);

            entity.Should().BeNull("Entity should be null!");    



        }



    }
}
