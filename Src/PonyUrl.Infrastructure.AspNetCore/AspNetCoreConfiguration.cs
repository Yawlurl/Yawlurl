using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PonyUrl.Infrastructure.AspNetCore.Authorization;
using PonyUrl.Infrastructure.AspNetCore.Models;
using PonyUrl.Infrastructure.MongoDb;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore
{
    public static class AspNetCoreConfiguration
    {
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
                    ValidIssuer = configuration["JwtIssuer"],
                    ValidAudience = configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });

            services.AddMvcCore().AddAuthorization();

            // MongoDb Identity
            services.AddIdentityMongoDbProvider<ApplicationUser>(options =>
            {
                options.ConnectionString = MongoDbConfiguration.GetMongoDbAppSettings(configuration).ConnectionString;
            });


        }

    }
}
