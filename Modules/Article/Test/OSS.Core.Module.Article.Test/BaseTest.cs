using Microsoft.AspNetCore.Builder;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;

namespace OSS.Core.Module.Article.Test
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

            _webAppBuilder.Services.Register<ArticleGlobalStarter>();
            _webApp.UseOssCore();
            
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