using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PonyUrl.Domain.Core;
using PonyUrl.Infrastructure.AspNetCore.Authorization;
using PonyUrl.Infrastructure.AspNetCore.Filters;
using PonyUrl.Infrastructure.AspNetCore.Models;
using PonyUrl.Infrastructure.MongoDb;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore
{
    public static class AspNetCoreConfiguration
    {
        #region Consts
        private const string MONGODB_IDENTITY_CONNECTION = "MongoDbIdentity";
        private const string JWT_ISSUER = "JwtIssuer";
        private const string JWT_KEY = "JwtKey";
        private const string ROUTER_DOMAIN = "RouterDomain";
        #endregion

        public static void ConfigureAspNetCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<JwtTokenBuilder>();

            // ===== Add Jwt Authentication ========
            //remove default claim
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = false;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration[JWT_ISSUER],
                    ValidAudience = configuration[JWT_ISSUER],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[JWT_KEY])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });

            services.AddMvcCore().AddAuthorization();

            // MongoDb Identity
            services.AddIdentityMongoDbProvider<ApplicationUser>(options =>
            {
                options.ConnectionString = configuration.GetConnectionString(MONGODB_IDENTITY_CONNECTION);
            });

            //Access HttpContext in Application Layer
            services.AddHttpContextAccessor();

            //Logging Configuration

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExecutionFilter));
            });

            //GlobalSettings
            services.AddSingleton<IGlobalSettings>(new GlobalSettings() { RouterDomain = Convert.ToString(configuration[ROUTER_DOMAIN]) });
        }

    }
}
