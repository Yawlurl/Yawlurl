using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YawlUrl.Application.ShortUrls.Commands;
using YawlUrl.Application.ShortUrls.Queries;
using YawlUrl.Infrastructure;
using YawlUrl.Infrastructure.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace YawlUrl.Web.Router
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.ConfigureGlobal(Configuration);

            //Add AspNetCore
            services.ConfigureAspNetCore(Configuration);

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(typeof(GetShortUrlQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAllShortUrlQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateShortUrlCommandHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteShortUrlCommandHandler).GetTypeInfo().Assembly);

            //Controllers
            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Router/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute("Root", "/{slug}", new { controller = "Router", action = "Index", slug = UrlParameter.Optional });
            });
        }
    }
}
