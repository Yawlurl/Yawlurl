using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PonyUrl.Infrastructure.Redis;
using FluentAssertions;
using System.Threading.Tasks;
using System;
using PonyUrl.Core;

namespace PonyUrl.Integration.Test.Redis
{
    public class RedisDbTest : BaseTest
    {
        const string rootUrl = "http://localhost:8080";

        readonly RedisTestModel model = new RedisTestModel(rootUrl);

        private readonly ICacheManager cacheManager;

        public RedisDbTest()
        {
            cacheManager = ServiceProvider.GetService<ICacheManager>();

            Task.FromResult(cacheManager.Clear()).Wait();
        }

        [Fact]
        public async Task RedisDb_Conntection_Test()
        {
            (await cacheManager.IsConnected()).Should().BeTrue();
        }

        [Fact]
        public async Task RedisDb_Add_Key_Test()
        {
            string key = "TestKey_" + Guid.NewGuid();

            //Remove TestKey
            (await cacheManager.Delete(key)).Should().BeFalse();

            //Check TestKey
            (await cacheManager.IsExist(key)).Should().BeFalse();

            //Add TestKey
            (await cacheManager.Set(key, model)).Should().BeTrue();

            //Check TestKey
            (await cacheManager.IsExist(key)).Should().BeTrue();

        }

        [Fact]
        public async Task RedisDb_Hash_Key_Test()
        {
            const string key = "120";

            if(await cacheManager.IsExistUrls())
            {
                //Clear Collection
                (await cacheManager.ClearUrls()).Should().BeTrue();
            }
           

            var keys = System.Linq.Enumerable.Range(100, 150);

            //Add ShortUrls
            foreach (var k in keys)
            {
                (await cacheManager.SetUrl(k.ToString(), $"{model.Url}/{k}")).Should().BeTrue();
            }

            //Check Url
            (await cacheManager.IsExistUrl(key)).Should().BeTrue();

            //Get Url
            (await cacheManager.GetUrl(key)).Should().Be($"{rootUrl}/{key}");
        }
    }
}
