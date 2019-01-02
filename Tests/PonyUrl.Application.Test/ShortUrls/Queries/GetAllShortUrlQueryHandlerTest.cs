using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Application.ShortUrls.Queries.GetAllShortUrl;
using PonyUrl.Domain.Entities;
using PonyUrl.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PonyUrl.Application.Test.ShortUrls.Queries
{
    public  class GetAllShortUrlQueryHandlerTest : TestBase
    {
        private IShortUrlRepository _shortUrlRepository;
        GetAllShortUrlQueryHandler _queryhandler;
        public GetAllShortUrlQueryHandlerTest()
        {
            var serviceProvider = services.BuildServiceProvider();

            _shortUrlRepository = serviceProvider.GetService<IShortUrlRepository>();

            _shortUrlRepository.InsertAsync(new ShortUrl("http://www.google.com"));
            _shortUrlRepository.InsertAsync(new ShortUrl("http://www.yahoo.com"));

            _queryhandler = new GetAllShortUrlQueryHandler(_shortUrlRepository);
        }

        [Fact]
        public async Task GetAllShortUrl_Test()
        {
           var result = await  _queryhandler.Handle(new GetAllShortUrlQuery(), CancellationToken.None);

            result.Should().BeOfType<ShortUrlListViewModel>();

            result.ShortUrls.ToList().Count.Should().Be(2, "Total count must be 2");
        }
    }
}
