using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal
{
    public class RoleRep : BasePortalRep<RoleMo>, IRoleRep
    {
        public RoleRep() : base("b_permit_role")
        {
        }
        
        #region 搜索角色表

        /// <summary>
        ///  搜索角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageList<RoleMo>> SearchRoles(SearchReq req)
        {
            return SimpleSearch(req);
        }
        protected override string BuildSimpleSearch_FilterItemSql(string key, string value,
            Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "name":
                    return " name LIKE '%" + SqlFilter(value.ToString()) + "%'";
            }

            return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
        }
        #endregion

        /// <summary>
        ///  通过角色Ids获取角色列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<List<RoleMo>> GetList(IList<long> ids)
        {
            var sql = string.Concat("select * from ", TableName, " where id in(", string.Join(",", ids),
                ") and status>@status ");
            var paras = new { status = (int)CommonStatus.Deleted };

            return GetList(sql, paras);
        }


        /// <summary>
        ///  通过角色id获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Task<Resp<RoleMo>> GetById(long id)
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Permit_Role_ById, id);
            var getFunc  = () => base.GetById(id);
            
            return getFunc.WithRespCacheAsync(cacheKey, TimeSpan.FromHours(2));
        }

        /// <summary>
        /// 修改角色状态
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Task<Resp> UpdateStatus(long rid, CommonStatus status)
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Permit_Role_ById, rid);

            return Update(u => new { u.status }, u => u.id == rid, new { status })
                .WithRespCacheClearAsync(cacheKey);
        }

        /// <summary>
        /// 修改角色名称
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Resp> UpdateName(long rid, string name)
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Permit_Role_ById, rid);

            return Update(u => new { name },
                    u => u.id == rid)
            .WithRespCacheClearAsync(cacheKey);
        }
    }
}
