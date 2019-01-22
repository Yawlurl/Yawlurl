using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Application.ShortUrls.Queries;
using PonyUrl.Domain;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PonyUrl.Application.Test.ShortUrls.Queries
{

    public class GetShortUrlQueryHandlerTest : TestBase
    {
        private IShortUrlRepository _shortUrlRepository;
        GetShortUrlQueryHandler _queryhandler;
        public GetShortUrlQueryHandlerTest()
        {
            var serviceProvider = services.BuildServiceProvider();

            _shortUrlRepository = serviceProvider.GetService<IShortUrlRepository>();

            _shortUrlRepository.InsertAsync(new ShortUrl("http://www.google.com"));
            _shortUrlRepository.InsertAsync(new ShortUrl("http://www.yahoo.com"));

            _queryhandler = new GetShortUrlQueryHandler(_shortUrlRepository);
        }

        [Fact]
        public async Task GetShortUrl_Test()
        {
            var list = await _shortUrlRepository.GetAllAsync();
            var id = list.Find(q => q.LongUrl == "http://www.google.com").Id;


            var result = await _queryhandler.Handle(new GetShortUrlQuery { Id = id }, CancellationToken.None);

            result.Should().BeOfType<ShortUrlViewModel>();

            result.LongUrl.Should().Be("http://www.google.com");

        }
    }
}
