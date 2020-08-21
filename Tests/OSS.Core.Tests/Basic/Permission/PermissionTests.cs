using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.Permit;
using OSS.Core.Services.Basic.Permit;
using Xunit;

namespace OSS.Core.Tests.Basic.Permission
{

    public class PermissionTests : BaseTests
    {
        private static readonly PermitService _perService = new PermitService();

        [Fact]
        public async Task GetAuthUserRolesTest() 
        {
            var res = await _perService.GetUserRoles(UserContext.Identity.id);
            Assert.True(res.IsSuccess()|| res.IsRespType(RespTypes.ObjectNull));
        }



        [Fact]
        public async Task GetAuthUserRoleFuncsTest()
        {
            var res = await _perService.GetAuthUserFuncList();
            Assert.True(res.IsSuccess() || res.IsRespType(RespTypes.ObjectNull));
        }


        [Fact]
        public async Task GetUserCountByRoleIdTest()
        {
            var res = await RoleUserRep.Instance.GetUserCountByRoleId("1");
            Assert.True(res.IsSuccess() || res.IsRespType(RespTypes.ObjectNull));
        }


        [Fact]
        public async Task ChangeRoleFuncsTest()
        {
            var addList = new List<string>() {"test", "test1"};
            // 测添加
            var addRes= await _perService.ChangeRoleFuncItems(UserContext.Identity.id, addList, null);
            Assert.True(addRes.IsSysOk());
            // 测删除
            var delRes = await _perService.ChangeRoleFuncItems(UserContext.Identity.id, null, addList);
            Assert.True(delRes.IsSysOk());

        }
    }
}
