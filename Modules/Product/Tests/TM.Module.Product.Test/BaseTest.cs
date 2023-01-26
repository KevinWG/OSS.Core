using Microsoft.AspNetCore.Builder;
using OSS.Core;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;
using TM.Module.Product;

namespace OSS.CoreModule.Product.Test
{
    [TestClass]
    public class BaseTest
    {
        protected static readonly WebApplicationBuilder _webAppBuilder   = WebApplication.CreateBuilder();
        protected static readonly WebApplication        _webApp          = _webAppBuilder.Build();

        [TestInitialize]
        public virtual void InitialTestContext()
        {
            ConfigHelper.Configuration = _webApp.Configuration;

            _webAppBuilder.Services.Register<ProductGlobalStarter>();
            _webApp.UseOssCore();
            
            CoreContext.App.Identity = new AppIdentity()
            {
                app_ver = "1.0"
            };

            CoreContext.Tenant.Identity = new TenantIdentity()
            {
                id = "1",
                name = "OSSCore工作室"
            };

            CoreContext.User.Identity = new UserIdentity()
            {
                id   = "1",
                name = "测试用户",
                auth_type = PortalAuthorizeType.Admin
            };
        }
    }
}