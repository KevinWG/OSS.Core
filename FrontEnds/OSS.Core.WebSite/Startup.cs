using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.WebSite.AppCodes.Filters;

namespace OSS.Core.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
          ConfigUtil.Configuration=  Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
                DefaultContentType = "image/x-icon"
            });

            app.UseExceptionMiddleware();
            app.UseSysAuthInfoMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
