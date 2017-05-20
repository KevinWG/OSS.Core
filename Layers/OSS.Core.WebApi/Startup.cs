using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using OSS.Core.DomainMos;
using OSS.Core.DomainMos.Members.Interfaces;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.RepDapper.Members;
using OSS.Core.WebApi.Filters;

namespace OSS.Core.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            ConfigUtil.Configuration = Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterRep();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        { 
            ConfigStaticFiles(app);

            app.UseAuthorizeSignMiddleware();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Home}/{action=Index}/{id?}");
            });
        }
        /// <summary>
        ///   处理默认和静态文件
        /// </summary>
        /// <param name="app"></param>
        private static void ConfigStaticFiles(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
                DefaultContentType = "image/x-icon"
            });
            app.UseStaticFiles();
        }


        /// <summary>
        /// 注册仓储接口的具体实现
        /// </summary>
        private static void RegisterRep()
        {
            Rep<IUserInfoRep>.Set<UserInfoRep>();
            Rep<IAdminInfoRep>.Set<AdminInfoRep>();
        }
    }


 


}
