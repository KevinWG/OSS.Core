using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Module.Portal;

namespace OSS.Core.Module.All.Test.Permission
{
    [TestClass]
    public class PermissionTests : BaseTest
    {
        private static readonly PermitService _perService  = new();
        private static readonly RoleService   _roleService = new();
        private static readonly FuncService   _funcService = new();

        [TestMethod]
        public async Task GetAuthUserRolesTest()
        {
            var res = await _roleService.GetUserRoles(CoreContext.User.Identity.id.ToInt64());
            Assert.IsTrue(res.IsSuccess() || res.IsRespCode(RespCodes.OperateObjectNull));
        }

        [TestMethod]
        public async Task GetAuthUserRoleFuncsTest()
        {
            var res = await _perService.GetCurrentUserPermits();
            Assert.IsTrue(res.IsSuccess() || res.IsRespCode(RespCodes.OperateObjectNull));
        }


        [TestMethod]
        public async Task GetUserCountByRoleIdTest()
        {
            var res = await InsContainer<IRoleUserRep>.Instance.GetUserCountByRoleId(1);
            Assert.IsTrue(res.IsSuccess() || res.IsRespCode(RespCodes.OperateObjectNull));
        }


        [TestMethod]
        public async Task ChangeRoleFuncsTest()
        {
            var addList = new List<string>() {"test", "test1"};
   
            // 测添加
            var addRes = await _perService.ChangeRolePermits(CoreContext.User.Identity.id.ToInt64(), new ChangeRolePermitReq()
            {
                add_items = addList
            });

            Assert.IsTrue(addRes.IsSysOk());
            // 测删除
            var delRes = await _perService.ChangeRolePermits(CoreContext.User.Identity.id.ToInt64(), new ChangeRolePermitReq()
            {
                delete_items = addList
            });
            Assert.IsTrue(delRes.IsSysOk());

        }

        [TestMethod]
        public async Task AddDefaultFunc()
        {
            var allFuncRes = await _funcService.GetAllFuncItems();
            Assert.IsTrue(allFuncRes.IsSuccess(), allFuncRes.msg);
        }


    }
}
