using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YawlUrl.Application.ShortUrls.Commands;
using YawlUrl.Application.ShortUrls.Queries;
using YawlUrl.Infrastructure;
using YawlUrl.Infrastructure.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;

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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //ExceptionFilters
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiExcepitonFilterAttribute));
            });


            //Controllers
            services.AddControllers();

        }


        private void ConfigureSwaggerOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Version = "v1",
                Title = "YawlUrl API",
                Description = $"YawlUrl Shortener service asp.net core {CompatibilityVersion.Version_3_0}"
            });


            var openApiRequirement = new OpenApiSecurityRequirement();
            //openApiRequirement.Add(new OpenApiSecurityScheme().


            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
            });
            options.AddSecurityRequirement(openApiRequirement);

            //options.OperationFilter<SwaggerHeaderFilter>();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //// ===== Use Authentication ======
            app.UseAuthentication();
            app.UseAuthorization();
            //ExceptionHandler
            app.ConfigureExceptionHandler();

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers(); 
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
