using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.CorePro.TAdminSite.Apis.Permit.Helpers;
using OSS.CorePro.TAdminSite.Apis.Permit.Reqs;
using OSS.CorePro.TAdminSite.Apis.Portal.Helpers;

namespace OSS.CorePro.TAdminSite.Apis.Permit
{
    public class PermitController : ProxyApiController
    {
        #region 角色处理
        
        /// <summary>
        ///  搜索角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleList)]
        public Task<IActionResult> SearchRoles()
        {
            return PostReqApi("/b/permit/SearchRoles");
        }

        /// <summary>
        ///   添加角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleAdd)]
        public Task<IActionResult> RoleAdd()
        {
            return PostReqApi("/b/permit/roleadd");
        }

        /// <summary>
        ///   更新角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleUpdate)]
        public Task<IActionResult> RoleUpdate()
        {
            return PostReqApi("/b/permit/RoleUpdate");
        }

        /// <summary>
        ///   启用角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleActive)]
        public Task<IActionResult> RoleActive(string rid)
        {
            return PostReqApi("/b/permit/RoleActive?rid=" + rid);
        }

        /// <summary>
        ///  作废角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleActive)]
        public Task<IActionResult> RoleUnActive(string rid)
        {
            return PostReqApi("/b/permit/RoleUnActive?rid=" + rid);
        }

        /// <summary>
        ///   删除角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleDelete)]
        public Task<IActionResult> RoleDelete(string rid)
        {
            return PostReqApi("/b/permit/RoleDelete?rid=" + rid);
        }
        
        #endregion

        #region 角色关联权限项管理

        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserFuncCode(FuncCodes.Permit_RoleFuncList)]
        public Task<ListResp<FuncBigItem>> GetAllFuncItems()
        {
            return FuncHelper.GetAllFuncItems();
        }

        /// <summary>
        ///  获取当前登录用户的权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserFuncCode(FuncCodes.None)]
        public async Task<ListResp<GetRoleItemResp>> GetAuthUserFuncList()
        {
            var memIdentityRes = await AdminHelper.GetAuthAdmin();
            if (!memIdentityRes.IsSuccess())
                return new ListResp<GetRoleItemResp>().WithResp(memIdentityRes);

            // 如果是超级管理员直接返回所有
            if (memIdentityRes.data.auth_type != PortalAuthorizeType.SuperAdmin)
                return await FuncHelper.GetAuthUserRoleFuncList();

            var sysFunItemsRes = await FuncHelper.GetAllFuncItems();
            if (!sysFunItemsRes.IsSuccess())
                return new ListResp<GetRoleItemResp>().WithResp(sysFunItemsRes);

            var roleItems = sysFunItemsRes.data.Select(item => item.ToRoleItemResp()).ToList();
            return new ListResp<GetRoleItemResp>(roleItems);
        }

        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserFuncCode(FuncCodes.Permit_RoleFuncList)]
        public Task<IActionResult> GetRoleFuncList(string rid)
        {
            return GetReqApi("/b/permit/GetRoleFuncList?rid="+ rid);
        }

        /// <summary>
        /// 获取角色对应权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleFuncChange)]
        public Task<IActionResult> ChangeRoleFuncItems(string rid)
        {
            return PostReqApi("/b/permit/ChangeRoleFuncItems?rid="+rid);
        }

        #endregion
        
        #region 角色关联用户管理
        
        /// <summary>
        ///   添加新的绑定
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleUserBind)]
        public Task<IActionResult> AddRoleBind()
        {
            return PostReqApi("/b/permit/AddRoleBind");
        }

        /// <summary>
        ///  搜索用户角色绑定关系
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleUserSearch)]
        public Task<IActionResult> SearchRoleUsers()
        {
            return PostReqApi("/b/permit/SearchRoleUsers");
        }

        /// <summary>
        ///   删除用户角色绑定关系
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Permit_RoleUserDelete)]
        public Task<IActionResult> DeleteRoleBind(string id)
        {
            return PostReqApi("/b/permit/DeleteRoleBind?id=" + id);
        }

        #endregion

    }
}
