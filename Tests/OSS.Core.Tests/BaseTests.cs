using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Services.Sys_Global;
using OSS.Tools.Config;

namespace OSS.Core.Tests
{
    public class BaseTests
    {
        static BaseTests()
        {
            SetConfig();

            GlobalRegister.RegisterConfig();

            InitialTestContext();
        }


        private static readonly string userId = "1";
        private static void InitialTestContext()
        {
            var appIdentity = new AppIdentity()
            {
                tenant_id = AppInfoHelper.SystemDefaultTenantId,
                app_id = AppInfoHelper.AppId,
                UDID = "TestDevice",
            };
            AppInfoHelper.FormatAppIdInfo(appIdentity);

            var userIdentity = new UserIdentity()
            {
                id = userId,
                auth_type = PortalAuthorizeType.Admin
            };

            AppReqContext.SetIdentity(appIdentity);
            UserContext.SetIdentity(userIdentity);
        }

        private static void SetConfig()
        {
            var basePat = Directory.GetCurrentDirectory();
            var sepChar = Path.DirectorySeparatorChar;
            var configPath = basePat.Substring(0, basePat.IndexOf(sepChar+"bin"+sepChar))+sepChar;
            var config = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .Add(new JsonConfigurationSource
                {
                    Path = "appsettings.json",
                    ReloadOnChange = true
                }).Build();

            ConfigHelper.Configuration = config;
        }

       
    }
}