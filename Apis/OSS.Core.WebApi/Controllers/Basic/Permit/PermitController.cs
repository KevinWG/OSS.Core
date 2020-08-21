using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.Core.RepDapper.Basic.Permit.Mos;
using OSS.Core.Services.Basic.Permit;
using OSS.Core.WebApi.Controllers.Basic.Permit.Reqs;

namespace OSS.Core.WebApi.Controllers.Basic.Permit
{
    [ModuleName(ModuleNames.Permit)]
    [Route("b/[controller]/[action]/{id?}")]
    public class PermitController:BaseController
    {
        private static readonly PermitService _service=new PermitService();

        #region 角色处理

        /// <summary>
        ///  搜索角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<PageListResp<RoleMo>> SearchRoles([FromBody] SearchReq req)
        {
            return req == null
                ? Task.FromResult(new PageListResp<RoleMo>().WithResp(ParaErrorResp))
                : _service.SearchRoles(req);
        }

        /// <summary>
        ///   删除角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IdResp<string>> RoleAdd([FromBody] AddRoleReq req)
        {
            return ModelState.IsValid
                ? _service.RoleAdd(req.ConvertToMo())
                : Task.FromResult(new IdResp<string>().WithResp(GetInvalidResp()));
        }

        /// <summary>
        ///  更新角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
        public Task<Resp> RoleActive(string rid)
        {
            return string.IsNullOrEmpty(rid)
                ? Task.FromResult(ParaErrorResp)
                : _service.RoleActive(rid);
        }

        /// <summary>
        ///  作废角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> RoleUnActive(string rid)
        {
            return string.IsNullOrEmpty(rid)
                ? Task.FromResult(ParaErrorResp)
                : _service.RoleUnActive(rid);
        }

        /// <summary>
        ///   删除角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> RoleDelete(string rid)
        {
            return string.IsNullOrEmpty(rid)
                ? Task.FromResult(ParaErrorResp)
                : _service.RoleDelete(rid);
        }

        #endregion

        #region 角色和功能项关联处理

        /// <summary>
        ///  获取登录用户的权限列表
        ///      （可能出现 10 分钟左右缓存误差）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ListResp<RoleFunSmallMo>> GetAuthUserFuncList()
        {
            return _service.GetAuthUserFuncList();
        }

        /// <summary>
        ///  获取当前角色下权限项列表
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public Task<ListResp<RoleFunSmallMo>> GetRoleFuncList(string rid)
        {
            return string.IsNullOrEmpty(rid)
                ? Task.FromResult(new ListResp<RoleFunSmallMo>().WithResp(ParaErrorResp))
                : _service.GetRoleFuncList(rid);
        }


        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> ChangeRoleFuncItems(string rid, [FromBody] ChangeRoleFuncItems items)
        {
            if (string.IsNullOrEmpty(rid)
                || items == null
                || !(items.add_items?.Count >0  || items.delete_items?.Count > 0))
                return Task.FromResult(ParaErrorResp);

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
        public Task<IdResp<string>> AddRoleBind([FromBody]AddRoleUserReq req)
        {
            return ModelState.IsValid
                ? _service.AddRoleBind(req.ToMo())
                : Task.FromResult(new IdResp<string>().WithResp(GetInvalidResp()));
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
        public Task<PageListResp<RoleUserBigMo>> SearchRoleUsers([FromBody] SearchReq req)
        {
            return req == null
                ? Task.FromResult(new PageListResp<RoleUserBigMo>().WithResp(ParaErrorResp))
                : _service.SearchRoleUsers(req);
        }

        /// <summary>
        ///  删除用户角色绑定关系
        /// </summary>
        /// <param name="id">绑定关系Id</param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> DeleteRoleBind(string id)
        {
            return string.IsNullOrEmpty(id)
                ? Task.FromResult(ParaErrorResp)
                : _service.DeleteRoleBind(id);
        }
        
        #endregion

    }
}
