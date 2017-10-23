using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.WebSite.Filters;

namespace OSS.Core.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ConfigUtil.Configuration = Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(op =>
            {
                //  设置属性序列化规范
                op.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 去除序列化中的默认值和空值
                op.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                op.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // 注册静态文件处理
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
                DefaultContentType = "image/x-icon"
            });

            // 注册异常处理
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionMiddleware();
            }
            //  注册全局信息处理
            app.UseSysAuthInfoMiddleware();

            //  站点路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "snsoauth",
                    template: "oauth/{action=auth}/{plat?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
              
            });
        }
    }
}
