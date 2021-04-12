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
            // ��Ϊ��ҪȫվУ���Ƿ��¼������������ȫ�ִ���
            // ����ӿ�Controller���ദ���ɣ�����ajax����ͳһ������Ȩ��¼��ת����ҳ��Ԫ�ر�������У�� 
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
            builder.AddRazorRuntimeCompilation(); //  ����״̬�¶�̬����
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
