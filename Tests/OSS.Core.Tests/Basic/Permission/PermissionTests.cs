using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.Permit;
using OSS.Core.Services.Basic.Permit;

namespace OSS.Core.Tests.Basic.Permission
{
    [TestClass]
    public class PermissionTests : BaseTests
    {
        private static readonly PermitService _perService = new PermitService();

        [TestMethod]
        public async Task GetAuthUserRolesTest() 
        {
            var res = await _perService.GetUserRoles(UserContext.Identity.id.ToInt64());
            Assert.IsTrue(res.IsSuccess()|| res.IsRespType(RespTypes.ObjectNull));
        }



        [TestMethod]
        public async Task GetAuthUserRoleFuncsTest()
        {
            var res = await _perService.GetMyFuncs();
            Assert.IsTrue(res.IsSuccess() || res.IsRespType(RespTypes.ObjectNull));
        }


        [TestMethod]
        public async Task GetUserCountByRoleIdTest()
        {
            var res = await RoleUserRep.Instance.GetUserCountByRoleId(1);
            Assert.IsTrue(res.IsSuccess() || res.IsRespType(RespTypes.ObjectNull));
        }


        [TestMethod]
        public async Task ChangeRoleFuncsTest()
        {
            var addList = new List<string>() {"test", "test1"};
            // 测添加
            var addRes= await _perService.ChangeRoleFuncItems(UserContext.Identity.id.ToInt64(), addList, null);
            Assert.IsTrue(addRes.IsSysOk());
            // 测删除
            var delRes = await _perService.ChangeRoleFuncItems(UserContext.Identity.id.ToInt64(), null, addList);
            Assert.IsTrue(delRes.IsSysOk());

        }
    }
}
