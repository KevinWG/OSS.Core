using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public class RoleService : IRoleService, IOpenedRoleService
    {
        private static readonly IRoleRep _roleRep = InsContainer<IRoleRep>.Instance;

        /// <summary>
        ///  添加角色
        /// </summary>
        /// <returns></returns>
        public async Task<IResp> Add(AddRoleReq req)
        {
            var rMo = req.ToMo();

            rMo.FormatBaseByContext();

            await _roleRep.Add(rMo);

            return Resp.DefaultSuccess;
        }

      

        /// <summary>
        ///   搜索角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageListResp<RoleMo>> Search(SearchReq req)
        {
            return new PageListResp<RoleMo>(await _roleRep.SearchRoles(req));
        }

        /// <summary>
        ///  更新角色名称
        /// </summary>
        /// <returns></returns>
        public async Task<IResp> UpdateName(long id, AddRoleReq req)
        {
            return await _roleRep.UpdateName(id, req.name);
        }

        /// <summary>
        ///   激活角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public Task<IResp> Active(long rid)
        {
            return _roleRep.UpdateStatus(rid, CommonStatus.Original);
        }

        /// <summary>
        ///  下线角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public async Task<IResp> UnActive(long rid)
        {
            var countRes = await InsContainer<IRoleUserRep>.Instance.GetUserCountByRoleId(rid); 
            if (!countRes.IsSuccess())
                return countRes;

            if (countRes.data == 0)
                return await _roleRep.UpdateStatus(rid, CommonStatus.UnActive);

            return new Resp(RespCodes.OperateFailed, "当前角色已绑定用户，请取消用户绑定后再操作！");
        }



        #region 角色用户绑定处理

        private static readonly IRoleUserRep _roleUserRep = InsContainer<IRoleUserRep>.Instance;

        ///// <summary>
        /////  搜索用户角色绑定关系
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //public async Task<PageListResp<RoleUserBigMo>> SearchRoleUsers(SearchReq req)
        //{
        //    return new PageListResp<RoleUserBigMo>(await _roleUserRep.SearchRoleUsers(req));
        //}



        /// <summary>
        ///  获取当前授权用户的角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<ListResp<RoleMo>> GetUserRoles(long userId)
        {
            var roleIds = await _roleUserRep.GetRoleIdsByUserId(userId);
            if (roleIds.Count == 0)
                return new ListResp<RoleMo>(new List<RoleMo>());

            var list = await _roleRep.GetList(roleIds);
            return new ListResp<RoleMo>(list);
        }



        /// <summary>
        ///   添加新的绑定
        /// </summary>
        /// <returns></returns>
        public async Task<IResp> UserBind(AddRoleUserReq req)
        {
            var ruMo = req.ToMo();

            ruMo.FormatBaseByContext();

            await _roleUserRep.Add(ruMo);

            return Resp.DefaultSuccess;
        }

        /// <summary>
        ///  删除用户角色绑定信息
        /// </summary>
        /// <returns></returns>
        public Task<IResp> DeleteUserBind(long userId,long roleId)
        {
            return _roleUserRep.DeleteBind(userId, roleId);
        }

        /// <inheritdoc />
        Task<List<long>> IRoleService.GetRoleIdsByUserId(long userId)
        {
            return _roleUserRep.GetRoleIdsByUserId(userId);
        }

        #endregion
    }
}
