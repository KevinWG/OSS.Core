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
            var changeRes = await _service.Lock(CoreContext.User.Identity.id.ToInt64());
            Assert.IsTrue(changeRes.IsSuccess());

            changeRes = await _service.UnLock(CoreContext.User.Identity.id.ToInt64());
            Assert.IsTrue(changeRes.IsSuccess());
        }

    }
}
