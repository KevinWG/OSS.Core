using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.Core.AdminSite.Apis.Permit.Reqs;
using OSS.Core.Infrastructure.Const;
using OSS.Core.RepDapper.Basic.Permit.Mos;
using OSS.Core.Services.Basic.Permit;
using OSS.Core.Services.Basic.Permit.Reqs;

namespace OSS.Core.CoreApi.Controllers.Basic.Permit
{
    [ModuleMeta(CoreModuleNames.Permit)]
    [Route("b/[controller]/[action]")]
    public class PermitController:BaseController
    {
        private static readonly PermitService _service = new PermitService();

        #region 角色处理

        /// <summary>
        ///  搜索角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleList)]
        public Task<PageListResp<RoleMo>> SearchRoles([FromBody] SearchReq req)
        {
            return req == null
                ? Task.FromResult(new PageListResp<RoleMo>().WithResp(GetInvalidResp()))
                : _service.SearchRoles(req);
        }

        /// <summary>
        ///   删除角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleAdd)]
        public Task<Resp<long>> RoleAdd([FromBody] AddRoleReq req)
        {
            return ModelState.IsValid
                ? _service.RoleAdd(req.ConvertToMo())
                : Task.FromResult(GetInvalidResp<long>());
        }

        /// <summary>
        ///  更新角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleUpdate)]
        public Task<Resp> RoleUpdate([FromBody] UpdateRoleReq req)
        {
            return ModelState.IsValid
                ? _service.RoleUpdate(req.id, req.name)
                : Task.FromResult(new Resp().WithResp(GetInvalidResp()));
        }

        /// <summary>
        ///   启用角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleActive)]
        public Task<Resp> RoleActive(long rid)
        {
            return rid <= 0
                ? Task.FromResult(GetInvalidResp())
                : _service.RoleActive(rid);
        }

        /// <summary>
        ///  作废角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleActive)]
        public Task<Resp> RoleUnActive(long rid)
        {
            return rid <= 0
                ? Task.FromResult(GetInvalidResp())
                : _service.RoleUnActive(rid);
        }

        /// <summary>
        ///   删除角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleDelete)]
        public Task<Resp> RoleDelete(long rid)
        {
            return rid <= 0
                ? Task.FromResult(GetInvalidResp())
                : _service.RoleDelete(rid);
        }

        #endregion

        #region 角色和功能项关联处理


        /// <summary>
        ///  获取系统所有权限项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserFunc(CoreFuncCodes.Permit_RoleFuncList)]
        public Task<ListResp<FuncBigItem>> GetAllFuncItems()
        {
            return _service.GetAllFuncItems();
        }

        /// <summary>
        ///  获取登录用户的权限列表
        ///      （可能出现 10 分钟左右缓存误差）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ListResp<RoleFunSmallMo>> GetMyFuncs()
        {
            return _service.GetMyFuncs();
        }



        /// <summary>
        ///  获取当前角色下权限项列表
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        [UserFunc(CoreFuncCodes.Permit_RoleFuncList)]
        public Task<ListResp<RoleFunSmallMo>> GetRoleFuncList(long rid)
        {
            return rid <= 0
                ? Task.FromResult(new ListResp<RoleFunSmallMo>().WithResp(GetInvalidResp()))
                : _service.GetRoleFuncList(rid);
        }


        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleFuncChange)]
        public Task<Resp> ChangeRoleFuncItems(long rid, [FromBody] ChangeRoleFuncItemsReq items)
        {
            if (rid <= 0
                || items == null
                || !(items.add_items?.Count > 0 || items.delete_items?.Count > 0))
                return Task.FromResult(GetInvalidResp());

            return _service.ChangeRoleFuncItems(rid, items.add_items, items.delete_items);
        }

        #endregion

        #region 角色用户绑定管理

        /// <summary>
        ///   添加新的绑定
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleUserBind)]
        public Task<Resp<long>> AddRoleBind([FromBody] AddRoleUserReq req)
        {
            return ModelState.IsValid
                ? _service.AddRoleBind(req.ToMo())
                : Task.FromResult(GetInvalidResp<long>());
        }

        /// <summary>
        ///  搜索用户角色绑定关系
        /// </summary>
        /// <param name="req">
        ///filter:
        /// r_name => 角色名称
        /// u_name => 用户名称    
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleUserSearch)]
        public Task<PageListResp<RoleUserBigMo>> SearchRoleUsers([FromBody] SearchReq req)
        {
            return req == null
                ? Task.FromResult(new PageListResp<RoleUserBigMo>().WithResp(GetInvalidResp()))
                : _service.SearchRoleUsers(req);
        }

        /// <summary>
        ///  删除用户角色绑定关系
        /// </summary>
        /// <param name="id">绑定关系Id</param>
        /// <returns></returns>
        [HttpPost]
        [UserFunc(CoreFuncCodes.Permit_RoleUserDelete)]
        public Task<Resp> DeleteRoleBind(long id)
        {
            return id <= 0
                ? Task.FromResult(GetInvalidResp())
                : _service.DeleteRoleBind(id);
        }

        #endregion
    }
}
