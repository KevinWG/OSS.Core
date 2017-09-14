#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 程序启动配置类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using OSS.Common.ComUtils;
using OSS.Core.Domains.Members.Interfaces;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.RepDapper.Members;
using OSS.Core.WebApi.Filters;

namespace OSS.Core.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ConfigUtil.Configuration = Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(op =>
            {
                op.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            RegisterRep();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseExceptionMiddleware();// 注册全局错误

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
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
                DefaultContentType = "image/x-icon"
            });
        }


        /// <summary>
        /// 注册仓储接口的具体实现
        /// </summary>
        private static void RegisterRep()
        {
            InsContainer<IUserInfoRep>.Set<UserInfoRep>();
            InsContainer<IAdminInfoRep>.Set<AdminInfoRep>();
        }
    }
}
