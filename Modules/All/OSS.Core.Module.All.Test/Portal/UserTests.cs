using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Module.Portal;

namespace OSS.Core.Module.All.Test.Portal
{
    [TestClass]
    public class UserTests : BaseTest
    {
        private static readonly UserService _service = new UserService();

        [TestMethod]
        public async Task GetUsersTest()
        {
            var usersRes = await _service.SearchUsers(new SearchReq());
            Assert.IsTrue(usersRes.IsSuccess());
        }
    }
}
