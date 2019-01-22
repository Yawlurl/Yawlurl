using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using AspNetCore.Identity.Mongo;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PonyUrl.Application.ShortUrls.Queries.GetShortUrl;
using PonyUrl.Domain.Interfaces;
using PonyUrl.Infrastructure.MongoDb;
using PonyUrl.Infrastructure.MongoDb.Identity.Models;
using PonyUrl.Infrastructure.MongoDb.Repository;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using PonyUrl.Infrastructure;

namespace PonyUrl.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Swagger
            services.AddSwaggerGen(ConfigureSwaggerOptions);

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>)); 
            services.AddMediatR(typeof(GetShortUrlQueryHandler).GetTypeInfo().Assembly);

            // MongoDb
            MongoDbConfiguration.ConfigureMongoDb(services, Configuration);

            // MongoDb Identity
            services.AddIdentityMongoDbProvider<ApplicationUser>(options =>
            {
                options.ConnectionString = MongoDbConfiguration.GetMongoDbAppSettings(Configuration).ConnectionString;
            });


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
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });


            // Add Dependencies 
            services.AddTransient<IShortUrlRepository, ShortUrlRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private void ConfigureSwaggerOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info()
            {
                Version = "v1",
                Title = "PonyUrl API",
                Description = "PonyUrl Shortener service asp.net core 2.2"
            });

            // Swagger 2.+ support
            var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

            options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = "header",
                Type = "apiKey"
            });
            options.AddSecurityRequirement(security);

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // ===== Use Authentication ======
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });


            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "PonyUrl API v1");
            });
        }
    }
}
