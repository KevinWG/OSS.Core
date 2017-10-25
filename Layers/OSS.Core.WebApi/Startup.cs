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
using Newtonsoft.Json.Serialization;
using OSS.Common.ComUtils;
using OSS.Core.Domains.Members.Interfaces;
using OSS.Core.Infrastructure.Plugs;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.RepDapper.Members;
using OSS.Core.Services.Sns.Exchange;
using OSS.Core.WebApi.Filters;
using OSS.Plugs.TemplateMsg.Email;
using OSS.Plugs.TemplateMsg.Sms;

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
            services.AddMvc(mvcOp => mvcOp.Filters.Add(new AuthorizeSignAttribute())).AddJsonOptions(op =>
            {
                //  设置属性序列化规范
                op.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 去除序列化中的默认值和空值
                op.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                op.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            });

            RegisteGlobal();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            ConfigStaticFiles(app);
            app.UseExceptionMiddleware(); // 注册全局错误
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}");

                //routes.MapRoute(
                //    name: "coreapi",
                //    template: "core/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
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
        /// 注册全局配置等相关实现
        /// </summary>
        private static void RegisteGlobal()
        {
            RegisteReps();
            RegistePlugs();
            RegisterWxConfig();
        }
        
        /// <summary>
        ///  注册仓储接口的具体实现
        /// </summary>
        private static void RegisteReps()
        {
            InsContainer<IUserInfoRep>.Set<UserInfoRep>();
            InsContainer<IAdminInfoRep>.Set<AdminInfoRep>();
            InsContainer<IOauthUserRep>.Set<OauthUserRep>();
        }

        /// <summary>
        ///  注册微信配置信息
        /// </summary>
        private static void RegisterWxConfig()
        {
            var appId = ConfigUtil.GetSection("DefaultWxConfig:AppId")?.Value;
            var appSecret = ConfigUtil.GetSection("DefaultWxConfig:AppSecret")?.Value;
            if (!string.IsNullOrEmpty(appId))
            {
                SnsOauthConfigProvider.RegisterDefaultWxConfig(appId,appSecret);
            }
        }
        
        /// <summary>
        /// 注册相关插件
        /// </summary>
        private static void RegistePlugs()
        {
            InsContainer<IEmailPlug>.Set<EmailPlug>();
            InsContainer<ISmsPlug>.Set<AliSmsPlug>();
        }
    }
}
