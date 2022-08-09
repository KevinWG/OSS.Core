using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Module.Portal;

namespace OSS.Core.Module.All.Test.Portal
{
    [TestClass]
    public class AdminTests : BaseTest
    {
        private static readonly AdminService _service = new();

        [TestMethod]
        public async Task GetUsersTest()
        {
            var adminRes = await _service.SearchAdmins(new SearchReq());
            Assert.IsTrue(adminRes.IsSuccess());
        }

        [TestMethod]
        public async Task ChangeStatusTest()
        {
            var changeRes = await _service.ChangeLockStatus(CoreContext.User.Identity.id.ToInt64(), true);
            Assert.IsTrue(changeRes.IsSuccess());

            changeRes = await _service.ChangeLockStatus(CoreContext.User.Identity.id.ToInt64(), false);
            Assert.IsTrue(changeRes.IsSuccess());
        }

    }
}
