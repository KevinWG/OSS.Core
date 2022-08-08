using Microsoft.AspNetCore.Builder;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Configuration;
using OSS.Core.Module.All.WebApi;

namespace OSS.Core.Module.All.Test
{
    [TestClass]
    public class BaseTest
    {
        protected static readonly WebApplicationBuilder _webAppBuilder   = WebApplication.CreateBuilder();
        protected static readonly WebApplication        _webApp          = _webAppBuilder.Build();

        [TestInitialize]
        public virtual void InitialTestContext()
        {

            _webAppBuilder.Services.AddOssCoreConfiguration(_webAppBuilder.Configuration);

            _webAppBuilder.Services.Register<AllWebApiStarter>();
            _webAppBuilder.Services.Register<AllWebUsedClientStarter>();
            
            _webApp.UseOssCore(new CoreContextOption()
            {
                JSRequestHeaderName = "x-core-app"
            });
            
            CoreContext.App.Identity = new AppIdentity()
            {
                app_ver = "1.0"
            };

            CoreContext.User.Identity = new UserIdentity()
            {
                id   = "1",
                name = "≤‚ ‘”√ªß",

                auth_type = PortalAuthorizeType.Admin
            };

        }
    }
}