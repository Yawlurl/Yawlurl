using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Application.Test.Infrastructure;
using PonyUrl.Domain.Interfaces;
using PonyUrl.Infrastructure.MongoDb;
using PonyUrl.Infrastructure.MongoDb.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Application.Test
{
    public class TestBase
    {
        public readonly MongoDbContext _mongoDbContext;
        public ServiceCollection services;
        public TestBase()
        {
            services = new ServiceCollection();

            _mongoDbContext = MongoDbContextFactory.Create(services); 
        }
 
    }
}
