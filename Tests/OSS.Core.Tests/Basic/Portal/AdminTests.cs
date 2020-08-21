using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Services.Basic.Portal;
using Xunit;

namespace OSS.Core.Tests.Basic.Portal
{

    public class AdminTests : BaseTests
    {
        private static readonly AdminService _service = new AdminService();

        [Fact]
        public async Task GetUsersTest()
        {
            var adminRes = await _service.SearchAdmins(new SearchReq()
            {
                filters = new Dictionary<string, string>
                {
                    {"admin_name", "测试管理员"}
                }
            });
            Assert.True(adminRes.IsSuccess());
        }


        [Fact]
        public async Task ChangeStatusTest()
        {
            var changeRes = await _service.ChangeLockStatus(UserContext.Identity.id,true);
            Assert.True(changeRes.IsSuccess()); 

            changeRes = await _service.ChangeLockStatus(UserContext.Identity.id, false);
            Assert.True(changeRes.IsSuccess());
        }

    }
}
