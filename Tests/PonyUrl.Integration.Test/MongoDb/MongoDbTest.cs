using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Infrastructure.MongoDb;
using System;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using FluentAssertions;

namespace PonyUrl.Integration.Test.MongoDb
{
    public class MongoDbTest : BaseTest
    {
        #region Properties
        private readonly TestEntity testEntity = new TestEntity()
        {
            Title = "Hello World!"
        };
        private readonly IMongoDbSettings _dbSettings;
        private readonly IMongoDbRepository<TestEntity> _mongoRepository;

        #endregion

        #region C'tor
        public MongoDbTest()
        {
            _dbSettings = ServiceProvider.GetService<IMongoDbSettings>();
            _mongoRepository = ServiceProvider.GetService<IMongoDbRepository<TestEntity>>();

        }
        #endregion

        [Fact]
        public void MongoDb_Conntection_Test()
        {
            (new MongoDbManager().ConnectionAvailable(_dbSettings)).Should().BeTrue();
        }

        [Fact]
        public async Task MongoDb_AsyncTest()
        {
            TestEntity testEntity;
            for (int i = 1; i <= 10; i++)
            {
                testEntity = new TestEntity($"Item{i}");

                await _mongoRepository.InsertAsync(testEntity);
            }

         (await _mongoRepository.GetCountAsync()).Should().BePositive();
        }

        [Fact]
        public async Task MongoDb_CRUD_Test()
        {
            //INSERT
            TestEntity entity = await _mongoRepository.InsertAsync(testEntity);

            entity.Id.Should().NotBeEmpty();

            //UPDATE
            entity.Title += " Updated";
            entity.UpdatedDate = DateTime.Now;

            await _mongoRepository.UpdateAsync(entity);

            entity.UpdatedDate.Should().BeAfter(entity.CreatedDate);

            //GET
            (await _mongoRepository.GetAllAsync()).Count.Should().BePositive();
            (await _mongoRepository.GetCountAsync()).Should().BePositive();
            (await _mongoRepository.GetAsync(entity.Id)).Title.Should().BeEquivalentTo(entity.Title);


            //DELETE
            (await _mongoRepository.DeleteAsync(entity.Id)).Should().BeTrue();
            (await _mongoRepository.GetAsync(entity.Id)).Should().BeNull();

        }


    }
}
