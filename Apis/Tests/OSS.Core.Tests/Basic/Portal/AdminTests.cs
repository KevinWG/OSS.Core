using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Common.BasicMos;
using OSS.Common.Resp;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.Services.Basic.Portal;

namespace OSS.Core.Tests.Basic.Portal
{
    [TestClass]
    public class AdminTests : BaseTests
    {
        private static readonly AdminService _service = new AdminService();

        [TestMethod]
        public async Task GetUsersTest()
        {
            var adminRes = await _service.SearchAdmins(new SearchReq()
            {
                filter = new Dictionary<string, string>
                {
                    {"admin_name", "测试管理员"}
                }
            });
            Assert.IsTrue(adminRes.IsSuccess());
        }


        [TestMethod]
        public async Task ChangeStatusTest()
        {
            var changeRes = await _service.ChangeLockStatus(CoreUserContext.Identity.id.ToInt64(),true);
            Assert.IsTrue(changeRes.IsSuccess()); 

            changeRes = await _service.ChangeLockStatus(CoreUserContext.Identity.id.ToInt64(), false);
            Assert.IsTrue(changeRes.IsSuccess());
        }

    }
}
