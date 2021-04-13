using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.Core.Services.Sys_Global;
using OSS.Core.WebApi.App_Codes.AuthProviders;
using OSS.Tools.Config;

namespace OSS.Core.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
           ConfigHelper.Configuration =  Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appOption=new AppAuthOption()
            {
                AppProvider =  new AppAuthProvider(),
                TenantProvider =new TenantAuthProvider()
            };
            var moduleOption=new ModuleAuthOption() { ModuleProvider = new ModuleAuthProvider()};
            var userOption=new UserAuthOption() { UserProvider = new UserAuthProvider() };

            services.AddControllers(
                    opt =>
                    {
                        opt.Filters.Add(new AppAuthAttribute(appOption));
                        opt.Filters.Add(new ModuleAuthAttribute(moduleOption));
                        opt.Filters.Add(new UserAuthAttribute(userOption));
                    })
                .AddJsonOptions(jsonOpt =>
                {
                    jsonOpt.JsonSerializerOptions.IgnoreNullValues     = true;
                    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppInfoHelper.EnvironmentName = env.EnvironmentName;
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseInitialMiddleware();
            if (!env.IsDevelopment())
            {
                app.UseExceptionMiddleware();
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            GlobalRegister.RegisterConfig();
        }
    }
}
