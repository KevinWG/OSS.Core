using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var appIdentity = new AppIdentity()
            {
                app_type = AppType.Single, tenant_id = "1"
            };

            var access = new AccessSecret("default_access", "60226eeaa3e949cfa84f2308e41e4775");
            var ticket = appIdentity.ToTicket(access.access_key, access.access_secret, "1.0");

            appIdentity.FormatFromTicket(ticket);

            var checkRes = appIdentity.CheckSign(access.access_secret, 3600);
            Assert.IsTrue(checkRes.IsSuccess(), checkRes.msg);
        }
    }
}