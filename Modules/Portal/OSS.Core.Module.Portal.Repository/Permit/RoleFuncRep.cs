using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal
{
    public class RoleFuncRep : BasePortalRep<RoleFuncMo>, IRoleFuncRep
    {
        public RoleFuncRep() : base("b_permit_role_func")
        {
        }

        /// <summary>
        ///  获取指定角色的权限列表
        /// </summary>
        /// <returns></returns>
        public Task<List<GrantedPermit>> GetRoleFuncList(long roleId)
        {
            var key = string.Concat(PortalConst.CacheKeys.Permit_RoleFuncs_ByRId, roleId);
            var sql = string.Concat("select * from ", TableName, " where role_id=@role_id and status>@status ");

            var getFunc = () => GetList<GrantedPermit>(sql, new {role_id = roleId, status = CommonStatus.Deleted});

            return getFunc.WithCacheAsync(key, TimeSpan.FromHours(2));
        }

        /// <summary>
        ///  获取多个角色的权限列表
        /// </summary>
        /// <returns></returns> 
        public Task<List<GrantedPermit>> GetRoleFuncList(List<long> roleIds)
        {
            var sql = string.Concat("select func_code,data_level from ", TableName, " where role_id in (",
                SqlFilter(string.Join(",", roleIds)),
                ") and status>@status");

            return GetList<GrantedPermit>(sql, new {status = CommonStatus.Deleted});
        }

        /// <summary>
        ///  修改关联的权限项
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="addItems"></param>
        /// <param name="deleteItems"></param>
        /// <returns></returns>
        public async Task<IResp> ChangeRoleFuncItems(long rid, List<RoleFuncMo>? addItems, List<string>? deleteItems)
        {
            var key = string.Concat(PortalConst.CacheKeys.Permit_RoleFuncs_ByRId, rid);

            if (deleteItems?.Count > 0)
            {
                var softDeleteWhereSql =
                    " role_id=@role_id and status>@status and func_code in ('" +
                    SqlFilter(string.Join(",", deleteItems)).Replace(",", "','") + "')";

                var sdRes = await SoftDelete(softDeleteWhereSql,
                    new {status = CommonStatus.Deleted, role_id = rid}).WithRespCacheClearAsync(key);

                if (!sdRes.IsSuccess())
                    return sdRes;
            }

            if (addItems?.Count>0)
            {
                await AddList(addItems);
                await CacheHelper.RemoveAsync(key);
            }
     
            return Resp.DefaultSuccess;
        }


    }
}
