using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal;

public class GrantService : IGrantOpenService
{
    #region 获取用户拥有的权限码

    /// <summary>
    ///  获取当前授权用户下所有角色对应的权限码
    ///    （ 10 分钟左右缓存误差）
    /// </summary>
    /// <returns></returns>
    public async Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        var userIdentity = CoreContext.User.Identity;
        if (userIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
        {
            // 如果是超级管理员直接返回所有
            var sysFunItemsRes = await InsContainer<IFuncService>.Instance.GetAllFuncItems();
            if (!sysFunItemsRes.IsSuccess())
                return new ListResp<GrantedPermit>().WithResp(sysFunItemsRes);

            var roleItems = sysFunItemsRes.data.Select(item => item.ToSmallMo()).ToList();
            return new ListResp<GrantedPermit>(roleItems);
        }

        var key     = string.Concat(PortalConst.CacheKeys.Permit_UserFuncs_ByUId, userIdentity.id);
        var getFunc = GetUserGrantedItemsByRoles;

        return await getFunc.WithRespCacheAsync(key, TimeSpan.FromMinutes(10));
    }

    private static async Task<ListResp<GrantedPermit>> GetUserGrantedItemsByRoles()
    {
        var userId = CoreContext.User.Identity.id.ToInt64();

        var roleIdsRes = await InsContainer<IRoleService>.Instance.GetRoleIdsByUserId(userId);
        if (roleIdsRes.Count == 0)
            return new ListResp<GrantedPermit>(); // 用户无具体权限信息

        var funcItems = await _roleFuncRep.GetRoleFuncList(roleIdsRes);

        // 去重，取数据权限最大值
        var dir = new Dictionary<string, GrantedPermit>();
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

        return new ListResp<GrantedPermit>(dir.Select(d => d.Value).ToList());
    }


    /// <summary>
    ///  判断登录用户是否具有某权限
    /// </summary>
    /// <param name="funcCode"></param>
    /// <returns></returns>
    public async Task<IResp<FuncDataLevel>> CheckPermit(string funcCode)
    {
        if (string.IsNullOrEmpty(funcCode))
            return new Resp<FuncDataLevel>(FuncDataLevel.All);

        var userFunc = await GetCurrentUserPermits();
        if (!userFunc.IsSuccess())
            return new Resp<FuncDataLevel>().WithResp(userFunc);

        var func = userFunc.data.FirstOrDefault(f => f.func_code == funcCode);

        if (func == null)
            return new Resp<FuncDataLevel>().WithResp(RespCodes.UserNoPermission, "无操作权限!");

        return new Resp<FuncDataLevel>(func.data_level);
    }


    #endregion


    #region 角色权限Func关联管理

    private static readonly IRoleFuncRep _roleFuncRep = InsContainer<IRoleFuncRep>.Instance;

    /// <summary>
    ///  获取当前角色下权限项列表
    /// </summary>
    /// <returns></returns>
    public async Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new ListResp<GrantedPermit>(await _roleFuncRep.GetRoleFuncList(roleId));
    }

    /// <summary>
    ///   修改角色下的关联权限
    /// </summary>
    /// <returns></returns>
    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        var addList = req.add_items?.Select(a =>
        {
            var rfItem = new RoleFuncMo { role_id = rid, func_code = a, data_level = FuncDataLevel.All };
            rfItem.FormatBaseByContext();
            return rfItem;
        }).ToList();

        return _roleFuncRep.ChangeRoleFuncItems(rid, addList, req.delete_items);
    }
    

    
    #endregion
}