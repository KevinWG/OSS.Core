using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Helpers;

namespace OSS.Core.Tests.SysSettingTests
{

    public class AppInfoUtilTests : BaseTests
    {
        [TestMethod]
        public void GenerateAppId()
        {
            var appId = AppInfoHelper.GenerateAppId(AppInfoHelper.SystemDefaultTenantId,AppType.System, AppClientType.Server);
            var appinfo = new AppIdentity()
            {
                app_id = appId
            };

            AppInfoHelper.FormatAppIdInfo(appinfo);
            Assert.IsTrue(appinfo.app_type == AppType.System);
        }

    }
}
