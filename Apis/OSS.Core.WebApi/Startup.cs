using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OSS.Core.Infrastructure;
using OSS.Core.Services.Sys_Global;
using OSS.Tools.Config;
using OSS.Tools.Http;
using System.Net.Http;
using OSS.Core.WebApi.App_Codes.AuthProviders;
using OSS.Core.Context.Attributes;

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
            services.AddHttpClient();

            var appOption = new AppAuthOption()
            {
                AppProvider = new AppAuthProvider(),
            };
            var userOption = new UserAuthOption()
            {
                UserProvider = new UserAuthProvider(),
                FuncProvider = new FuncAuthProvider()
            };
          
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AppAuthAttribute(appOption));
                opt.Filters.Add(new UserAuthAttribute(userOption));
            }).AddJsonOptions(jsonOpt =>
            {
                jsonOpt.JsonSerializerOptions.IgnoreNullValues = true;
                jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpClientFactory clientFactory)
        {
            AppInfoHelper.EnvironmentName = env.EnvironmentName;
            HttpClientHelper.HttpClientFactory = clientFactory;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseOssCore(new CoreContextOption()
            {
                JSRequestHeaderName = "x-core-app"
            });

            if (!env.IsDevelopment())
            {
                app.UseOssCoreException();
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            GlobalRegister.RegisterConfig();
        }
    }
}
