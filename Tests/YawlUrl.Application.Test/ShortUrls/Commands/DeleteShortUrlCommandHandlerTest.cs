using YawlUrl.Application.ShortUrls.Commands;
using YawlUrl.Domain;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Threading;


namespace YawlUrl.Application.Test.ShortUrls.Commands
{
    public class DeleteShortUrlCommandHandlerTest : BaseTest
    {
        private readonly CreateShortUrlCommandHandler _createCommandHandler;
        private readonly DeleteShortUrlCommandHandler _deleteCommandHandler;


        public DeleteShortUrlCommandHandlerTest()
        {

            _createCommandHandler = new CreateShortUrlCommandHandler(SlugManagerMock,
                                                               HttpContextAccessorMock,
                                                               MediatorMock,
                                                               GlobalSettingsMock,
                                                               ShortUrlRepositoryMock,
                                                               UserManagerMock);

            _deleteCommandHandler = new DeleteShortUrlCommandHandler(ShortUrlRepositoryMock,
                                                                     MediatorMock,
                                                                     HttpContextAccessorMock,
                                                                     UserManagerMock,
                                                                     SlugRepositoryMock);
        }


        [Fact]
        public async Task DeleteShortUrl()
        {
            var shortUrl = await _createCommandHandler.Handle(new CreateShortUrlCommand()
            {
                LongUrl = "http://www.abc.com"
            },
            CancellationToken.None);

            shortUrl.Should().NotBeNull();
            shortUrl.SlugKey.Should().NotBeNullOrEmpty();

            var slugId = await That<ISlugManager>().GetSlugIdByKeyword(shortUrl.SlugKey);

            slugId.Should().NotBeEmpty();

            //Delete shorturl
            await _deleteCommandHandler.Handle(new DeleteShortUrlCommand() { SlugKey = shortUrl.SlugKey }, CancellationToken.None);

            (await That<IShortUrlRepository>().GetBySlug(slugId)).Should().BeNull();

            (await That<ISlugRepository>().GetByKey(shortUrl.SlugKey)).Should().BeNull("Entity should be null!");

        }



    }
}
