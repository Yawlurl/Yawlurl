using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyUrl.Infrastructure.MongoDb;
using PonyUrl.Infrastructure;
using System.IO;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using PonyUrl.Infrastructure.AspNetCore;
using Microsoft.AspNetCore.Identity;
using PonyUrl.Infrastructure.AspNetCore.Models;
using System.Threading.Tasks;
using MediatR;
using PonyUrl.Domain;
using PonyUrl.Domain.Core;

namespace PonyUrl.Application.Test
{
    public class BaseTest
    {
        public readonly MongoDbContext _mongoDbContext;
        public readonly ServiceCollection services;
        public readonly ServiceProvider serviceProvider;


        public IHttpContextAccessor HttpContextAccessorMock;
        public UserManager<ApplicationUser> UserManagerMock;
        public IMediator MediatorMock;
        public ISlugManager SlugManagerMock;
        public IShortUrlRepository ShortUrlRepositoryMock;
        public ISlugRepository SlugRepositoryMock;
        public IGlobalSettings GlobalSettingsMock;
        const string UserName = "TestUser";
        Guid UserId = Guid.Parse("0b7661cc-081c-4e7a-9c77-c7d26f7b5508");
        public BaseTest()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json",
                                                    optional: false, reloadOnChange: true);

            IConfigurationRoot configurationRoot = builder.Build();

            services = new ServiceCollection();

            services.ConfigureGlobal(configurationRoot);

            InitializeHttpContext();
           
            MediatorMock = new Mock<IMediator>().Object;
           
            SlugRepositoryMock = new Mock<ISlugRepository>().Object;
            ShortUrlRepositoryMock = new Mock<IShortUrlRepository>().Object;
            GlobalSettingsMock = new Mock<IGlobalSettings>().Object;

            SlugManagerMock = new Mock<SlugManager>(SlugRepositoryMock, ShortUrlRepositoryMock).Object;

            serviceProvider = services.BuildServiceProvider();
        }

        public T That<T>()
        {
            return serviceProvider.GetService<T>();
        }

        private void InitializeHttpContext()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var fakeHttpContext = new Mock<HttpContext>();
            var identity = new GenericIdentity(UserName, "Bearer");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, UserName),
                new Claim(JwtRegisteredClaimNames.Email, "test@test.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, string.Join(';', new string[]{"user"})),
                new Claim(ClaimTypes.AuthenticationMethod, AuthConstants.AuthenticationSchemes.Bearer),
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };
            identity.AddClaims(claims);

            var pricipal = new GenericPrincipal(identity, new string[] { "user" });
            fakeHttpContext.Setup(t => t.User).Returns(pricipal);

            var fakeUser = new Mock<ApplicationUser>();
            fakeUser.Setup(t => t.UserName).Returns(UserName);
            fakeUser.Setup(t => t.Id).Returns(UserId.ToString());
            
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            fakeUserManager.Setup(t => t.FindByNameAsync(UserName)).Returns(Task.FromResult(fakeUser.Object));
            mockHttpContextAccessor.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
       
            UserManagerMock = fakeUserManager.Object;
           
            HttpContextAccessorMock = mockHttpContextAccessor.Object;
        }

    }
}
