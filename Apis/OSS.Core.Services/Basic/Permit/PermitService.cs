using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.AdminSite.Apis.Permit.Helpers;
using OSS.Core.AdminSite.Apis.Permit.Reqs;
using OSS.Core.Infrastructure.BasicMos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Extensions;
using OSS.Core.RepDapper.Basic.Permit;
using OSS.Core.RepDapper.Basic.Permit.Mos;
using OSS.Core.Services.Basic.Permit.Proxy;
using OSS.Common.Extension;

namespace OSS.Core.Services.Basic.Permit
{
    public class PermitService: IPermitService
    {
        #region 角色查询修改处理 

        /// <summary>
        ///  添加角色
        /// </summary>
        /// <param name="rMo"></param>
        /// <returns></returns>
        public Task<Resp<long>> RoleAdd(RoleMo rMo)
        {
            rMo.InitialBaseFromContext();
            return RoleRep.Instance.Add(rMo);
        }

        /// <summary>
        ///  获取当前授权用户的角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<ListResp<RoleMo>> GetUserRoles(long userId)
        {
            var roleIdsRes = await RoleUserRep.Instance.GetRoleIdsByUserId(userId);
            if (!roleIdsRes.IsSuccess())
                return new ListResp<RoleMo>().WithResp(roleIdsRes);

            return await RoleRep.Instance.GetList(roleIdsRes.data);
        }

        /// <summary>
        ///   搜索角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<RoleMo>> SearchRoles(SearchReq req)
        {
            return RoleRep.Instance.SearchRoles(req);
        }

        /// <summary>
        ///  更新角色名称
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Resp> RoleUpdate(long rid, string name)
        {
            return await RoleRep.Instance.UpdateName(rid, name);
        }

        /// <summary>
        ///   激活角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public Task<Resp> RoleActive(long rid)
        {
            return RoleRep.Instance.UpdateStatus(rid, CommonStatus.Original);
        }

        /// <summary>
        ///   激活角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public async Task<Resp> RoleUnActive(long rid)
        {
            var countRes = await RoleUserRep.Instance.GetUserCountByRoleId(rid);
            if (!countRes.IsSuccess())
                return countRes;

            if (countRes.data == 0)
            {
                return await RoleRep.Instance.UpdateStatus(rid, CommonStatus.UnActived);
            }
            return new Resp(RespTypes.ObjectStateError, "当前角色已绑定用户，请取消用户绑定后再操作！");
        }

        /// <summary>
        ///   删除角色
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public Task<Resp> RoleDelete(long rid)
        {
            return RoleRep.Instance.UpdateStatus(rid, CommonStatus.Deleted);
        }

        #endregion

        #region 角色权限Func关联管理

        /// <summary>
        ///  获取当前角色下权限项列表
        /// </summary>
        /// <returns></returns>
        public Task<ListResp<RoleFunSmallMo>> GetRoleFuncList(long roleId)
        {
            return RoleFuncRep.Instance.GetRoleFuncList(roleId);
        }

        /// <summary>
        ///   修改角色下的关联权限
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="add_items"></param>
        /// <param name="delete_items"></param>
        /// <returns></returns>
        public Task<Resp> ChangeRoleFuncItems(long rid, List<string> add_items, List<string> delete_items)
        {
            var addList = add_items?.Select(a =>
            {
                var rfItem = new RoleFuncMo { role_id = rid, func_code = a, data_level = FuncDataLevel.All };
                rfItem.InitialBaseFromContext();
                return rfItem;
            }).ToList();

            return RoleFuncRep.Instance.ChangeRoleFuncItems(rid, addList, delete_items);
        }

        /// <summary>
        ///  判断登录用户是否具有某权限
        /// </summary>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        public async Task<Resp> CheckIfHaveFunc(string funcCode)
        {
            var memIdentity = UserContext.Identity;
            if (memIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
                return new Resp();

            var userFunc = await GetMyFuncs();
            if (userFunc.IsSuccess() && userFunc.data.Any(f => f.func_code == funcCode))
                return new Resp();

            return new Resp().WithResp(RespTypes.NoPermission, "无此权限！");
        }

        /// <summary>
        ///  获取当前授权用户下所有角色对应的全部权限列表
        ///    （ 10 分钟左右缓存误差）
        /// </summary>
        /// <returns></returns>
        public Task<ListResp<RoleFunSmallMo>> GetMyFuncs()
        {
            var userIdentity = UserContext.Identity;
            var key = string.Concat(CacheKeys.Perm_UserFuncs_ByUId, userIdentity.id);

            Func<Task<ListResp<RoleFunSmallMo>>> getFunc = () => GetUserAllFuncsNoCache();

            return getFunc.WithAbsoluteCache(key, TimeSpan.FromMinutes(10));
        }
        /// <summary>
        ///  获取系统所有权限项
        /// </summary>
        /// <returns></returns>
        public Task<ListResp<FuncBigItem>> GetAllFuncItems()
        {
            return FuncHelper.GetAllFuncItems();
        }


        private async Task<ListResp<RoleFunSmallMo>> GetUserAllFuncsNoCache()
        {
            var userIdentity = UserContext.Identity;
            if (userIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
            {
                // 如果是超级管理员直接返回所有
                var sysFunItemsRes = await FuncHelper.GetAllFuncItems();
                if (!sysFunItemsRes.IsSuccess())
                    return new ListResp<RoleFunSmallMo>().WithResp(sysFunItemsRes);

                var roleItems = sysFunItemsRes.data.Select(item => item.ToSmallMo()).ToList();
                return new ListResp<RoleFunSmallMo>(roleItems);
            }

            var userId = userIdentity.id.ToInt64();
            var roleIdsRes = await GetUserRoles(userId);
            if (!roleIdsRes.IsSuccess())
                return new ListResp<RoleFunSmallMo>().WithResp(roleIdsRes);

            var funcRes = await RoleFuncRep.Instance.GetRoleFuncList(roleIdsRes.data.Select(s => s.id).ToList());
            if (!funcRes.IsSuccess())
                return new ListResp<RoleFunSmallMo>().WithResp(funcRes, "未获取有效角色对应权限信息！");

            var funcItems = funcRes.data;
            var dir = new Dictionary<string, RoleFunSmallMo>();
            foreach (var f in funcItems)
            {
                if (!dir.ContainsKey(f.func_code))
                {
                    dir.Add(f.func_code, f);
                    continue;
                }

                var fValue = dir[f.func_code];
                if (f.data_level < fValue.data_level)
                {
                    fValue.data_level = f.data_level;
                }
            }

            return new ListResp<RoleFunSmallMo>(dir.Select(d => d.Value).ToList());
        }

        #endregion

        #region 角色用户绑定管理

        /// <summary>
        ///  搜索用户角色绑定关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<RoleUserBigMo>> SearchRoleUsers(SearchReq req)
        {
            return RoleUserRep.Instance.SearchRoleUsers(req);
        }


        /// <summary>
        ///   添加新的绑定
        /// </summary>
        /// <param name="ruMo"></param>
        /// <returns></returns>
        public Task<Resp<long>> AddRoleBind(RoleUserMo ruMo)
        {
            ruMo.InitialBaseFromContext();
            return RoleUserRep.Instance.Add(ruMo);
        }

        /// <summary>
        ///   重新启用绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Resp> DeleteRoleBind(long id)
        {
            return UpdateRoleUserStatus(id, CommonStatus.Deleted);
        }

        private Task<Resp> UpdateRoleUserStatus(long id, CommonStatus status)
        {
            return RoleUserRep.Instance.UpdateStatus(id, status);
        }


        #endregion

    }
}
 