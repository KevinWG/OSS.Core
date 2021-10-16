using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure;
using OSS.Core.RepDapper.Basic.Permit.Mos;

namespace OSS.Core.RepDapper.Basic.Permit
{
    public class RoleUserRep : BaseRep<RoleUserRep, RoleUserMo>
    {
        protected override string GetTableName()
        {
            return "b_permit_role_user";
        }
        
        /// <summary>
        ///  获取当前授权用户的功能权限列表  
        /// </summary>
        /// <returns></returns>
        public Task<ListResp<string>> GetRoleIdsByUserId(long userId)
        {
            var sql = string.Concat("select role_id from ", TableName,
                " where u_id=@u_id and status>@status");

            var cacheKey = string.Concat(CoreCacheKeys.Perm_UserRoles_ByUId, userId);

            Func<Task<ListResp<string>>> getFunc = () =>
                GetList<string>(sql, new {u_id = userId,  status = (int) CommonStatus.Deleted});

            return getFunc.WithAbsoluteCache(cacheKey, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        ///  通过角色Id获取用户数量
        ///  【主要在管理端用，暂无缓存】
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<Resp<int>> GetUserCountByRoleId(long roleId)
        {
            var sql = string.Concat("select count(1) from ", TableName,
                " where role_id=@role_id and status>@status");

            return Get<int>(sql, new {role_id = roleId, status = (int) CommonStatus.Deleted});
        }

        /// <summary>
        ///  修改绑定关系状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Task<Resp> UpdateStatus(long id, CommonStatus status)
        {
            return Update(u=>new {u.status},w=>w.id==id ,new{status});
        }
        
        /// <summary>
        ///  搜索用户角色绑定关联
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<RoleUserBigMo>> SearchRoleUsers(SearchReq req)
        {
            return SimpleSearch<RoleUserBigMo>(req);
        }

        protected override string BuildSimpleSearchSelectColumns(SearchReq req)
        {
            return "t.*,rt.name r_name";
        }

        protected override string BuildSimpleSearchTableName(SearchReq req)
        {
            //  可能涉及领域内表关联
            var tableSql = new StringBuilder(TableName).Append(" t")
                .Append(" inner join ").Append(RoleRep.Instance.TableName).Append(" rt")
                .Append(" on t.role_id=rt.id");

            return tableSql.ToString();
        }

        protected override string BuildSimpleSearchWhereSqlByFilterItem(string key, string value,
            Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "r_name":
                    return "rt.`name` LIKE '%" + SqlFilter(value.ToString()) + "%'";

                case "u_name":
                    return "t.`u_name` LIKE '%" + SqlFilter(value.ToString()) + "%'";
            }
            return base.BuildSimpleSearchWhereSqlByFilterItem(key, value, sqlParas);
        }
    }
}
