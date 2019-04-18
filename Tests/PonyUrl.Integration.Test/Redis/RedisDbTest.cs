using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PonyUrl.Infrastructure.Redis;
using FluentAssertions;
using System.Threading.Tasks;
using System;

namespace PonyUrl.Integration.Test.Redis
{
    public class RedisDbTest : BaseTest
    {
        const string rootUrl = "http://localhost:8080";
        readonly RedisTestModel model = new RedisTestModel(rootUrl);

        private readonly RedisManager redisManager;

        public RedisDbTest()
        {
            redisManager = ServiceProvider.GetService<RedisManager>();

            Task.FromResult(redisManager.FlushDb()).Wait();
        }

        [Fact]
        public void RedisDb_Conntection_Test()
        {
            redisManager.IsConnected.Should().BeTrue();
        }

        [Fact]
        public async Task RedisDb_Add_Key_Test()
        {
            (await redisManager.Set(Guid.NewGuid().ToString(), model)).Should().BeTrue();
        }

        [Fact]
        public async Task RedisDb_Hash_Key_Test()
        {
            const string collection = "ShortUrls";
            const string key = "12000";

            await redisManager.Delete(collection);

            var keys = System.Linq.Enumerable.Range(10000, 15000);

            foreach (var k in keys)
            {
                (await redisManager.SetHashKey(collection, k.ToString(), $"{model.Url}/{k}")).Should().BeTrue();
            }


            (await redisManager.IsExistHashKeyField(collection, key)).Should().BeTrue();

            (await redisManager.GetHashKeyValue(collection, key)).Should().Be($"{rootUrl}/{key}");

        }
    }
}
