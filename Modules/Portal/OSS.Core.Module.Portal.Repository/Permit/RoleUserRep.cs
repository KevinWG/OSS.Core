using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;
using System.Text;

namespace OSS.Core.Module.Portal
{
    public class RoleUserRep : BasePortalRep< RoleUserMo>, IRoleUserRep
    {
        public RoleUserRep() : base("b_permit_role_user")
        {
        }


        /// <summary>
        ///  通过角色Id获取用户数量
        ///  【主要在管理端用，暂无缓存】
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<IResp<int>> GetUserCountByRoleId(long roleId)
        {
            var sql = string.Concat("select count(1) from ", TableName,
                " where role_id=@role_id and status>@status");

            return Get<int>(sql, new { role_id = roleId, status = (int)CommonStatus.Deleted });
        }

        /// <summary>
        ///  获取当前授权用户的功能权限列表
        /// </summary>
        /// <returns></returns>
        public  Task<List<long>> GetRoleIdsByUserId(long userId)
        {
            var sql = string.Concat("select role_id from ", TableName,  " where u_id=@u_id and status>@status");

            var cacheKey = string.Concat(PortalConst.CacheKeys.Permit_UserRoles_ByUId, userId);

            var getFunc = () => GetList<long>(sql, new {u_id = userId, status = (int) CommonStatus.Deleted});
            return getFunc.WithCacheAsync(cacheKey, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        ///  添加绑定信息
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public override async Task Add(RoleUserMo mo)
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Permit_UserRoles_ByUId, mo.u_id);
            await base.Add(mo);

            await CacheHelper.RemoveAsync(cacheKey);
        }

        /// <summary>
        ///  删除绑定信息
        /// </summary>
        /// <returns></returns>
        public Task<IResp> DeleteBind(long userId,long roleId)
        {
            return Update(u => new { u.status }, w => w.u_id== userId&& w.role_id== roleId, new { status=CommonStatus.Deleted })
                .WithRespCacheClearAsync(string.Concat(PortalConst.CacheKeys.Permit_UserRoles_ByUId, userId));
        }

        ///// <summary>
        /////  搜索用户角色绑定关联
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //public Task<PageList<RoleUserBigMo>> SearchRoleUsers(SearchReq req)
        //{
        //    return SimpleSearch<RoleUserBigMo>(req);
        //}

        //protected override string BuildSimpleSearch_SelectColumns(SearchReq req)
        //{
        //    return "t.*,rt.name r_name";
        //}

        //protected override string BuildSimpleSearch_TableName(SearchReq req, Dictionary<string, object> paras)
        //{
        //    //  可能涉及领域内表关联
        //    var tableSql = new StringBuilder(TableName).Append(" t")
        //        .Append(" inner join b_permit_role rt")
        //        .Append(" on t.role_id=rt.id");

        //    return tableSql.ToString();
        //}

        //protected override string BuildSimpleSearch_FilterItemSql(string key, string value,
        //    Dictionary<string, object> sqlParas)
        //{
        //    switch (key)
        //    {
        //        case "r_name":
        //            return "rt.`name` LIKE '%" + SqlFilter(value.ToString()) + "%'";

        //        case "u_name":
        //            return "t.`u_name` LIKE '%" + SqlFilter(value.ToString()) + "%'";
        //    }
        //    return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
        //}

    
    }
}
