using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Helpers;
using Xunit;

namespace OSS.Core.Tests.SysSettingTests
{

    public class AppInfoUtilTests : BaseTests
    {
        [Fact]
        public void GenerateAppId()
        {
            var appId = AppInfoHelper.GenerateAppId(AppInfoHelper.SystemDefaultTenantId,AppType.System, AppClientType.Server);

            var appinfo = new AppIdentity()
            {
                app_id = appId
            };

            AppInfoHelper.FormatAppIdInfo(appinfo);
            Assert.True(appinfo.app_type == AppType.System);

            appId = AppInfoHelper.GenerateAppId(AppInfoHelper.SystemDefaultTenantId,AppType.Proxy, AppClientType.Server);
            appinfo = new AppIdentity()
            {
                app_id = appId
            };

            AppInfoHelper.FormatAppIdInfo(appinfo);
            Assert.True(appinfo.app_type == AppType.Proxy);
        }

    }
}
