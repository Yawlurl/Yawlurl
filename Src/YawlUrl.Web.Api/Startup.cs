using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YawlUrl.Application.ShortUrls.Commands;
using YawlUrl.Application.ShortUrls.Queries;
using YawlUrl.Infrastructure;
using YawlUrl.Infrastructure.AspNetCore;
using YawlUrl.Web.Api.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace YawlUrl.Web.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Swagger
            services.AddSwaggerGen(ConfigureSwaggerOptions);

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(typeof(GetShortUrlQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAllShortUrlQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateShortUrlCommandHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteShortUrlCommandHandler).GetTypeInfo().Assembly);


            //Add AspNetCore
            services.ConfigureAspNetCore(Configuration);

            //Add global configuration
            services.ConfigureGlobal(Configuration);

            //Add Routing
            services.Configure<RouteOptions>(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //ExceptionFilters
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiExcepitonFilterAttribute));
            });

        }


        private void ConfigureSwaggerOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info()
            {
                Version = "v1",
                Title = "YawlUrl API",
                Description = "YawlUrl Shortener service asp.net core 2.2"
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

            options.OperationFilter<SwaggerHeaderFilter>();

            // Set the comments path for the Swagger JSON and UI.
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath);
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            //// ===== Use Authentication ======
            app.UseAuthentication();

            //ExceptionHandler
            app.ConfigureExceptionHandler();

            //app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });


            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "YawlUrl API");
                //s.RoutePrefix = string.Empty;
            });
        }
    }
}
