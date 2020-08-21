using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Services.Basic.Portal;
using Xunit;

namespace OSS.Core.Tests.Basic.Portal
{

    public class UserTests : BaseTests
    {
        private static readonly UserService _service = new UserService();

        [Fact]
        public async Task GetUsersTest()
        {
            var usersRes = await _service.SearchUsers(new SearchReq());
            Assert.True(usersRes.IsSuccess());
        }

    }
}
