using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OSS.Core.Infrastructure.Web.Attributes;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.CorePro.TAdminSite.AppCodes.Initial;
using OSS.Tools.Config;

namespace OSS.CorePro.TAdminSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ConfigHelper.Configuration = Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appOption = new AppAuthOption()
            {
                TenantProvider = new TenantAuthProvider()
            };
            var userOption = new UserAuthOption()
            {
                UserProvider = new AdminAuthProvider(),
                FuncProvider = new FuncAuthProvider()
            };
            // 因为需要全站校验是否登录，所以这里是全局处理
            // 否则接口Controller基类处理即可，所有ajax请求统一处理，授权登录跳转，纯页面元素本身无需校验 
            services.AddControllers(opt =>
                {
                    opt.Filters.Add(new AppAuthAttribute(appOption));
                    opt.Filters.Add(new UserAuthAttribute(userOption));
                })
                .AddNewtonsoftJson(jsonOpt =>
                {
                    jsonOpt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    jsonOpt.SerializerSettings.ContractResolver  = new DefaultContractResolver();
                });

            var builder = services.AddRazorPages();
#if DEBUG
            builder.AddRazorRuntimeCompilation(); //  调试状态下动态编译
#endif

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/antd_spa"; });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            if (!env.IsDevelopment())
            {
                app.UseExceptionMiddleware();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                //spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8000");
                }
            });
        }
    }
}
