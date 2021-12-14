using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Common.BasicMos;
using OSS.Common.Resp;
using OSS.Core.Services.Basic.Portal;

namespace OSS.Core.Tests.Basic.Portal
{
    [TestClass]
    public class UserTests : BaseTests
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
