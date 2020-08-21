using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Extensions;
using OSS.Core.RepDapper.Basic.Permit.Mos;
using OSS.Tools.Cache;

namespace OSS.Core.RepDapper.Basic.Permit
{
    public class RoleRep : BaseTenantRep<RoleRep, RoleMo>
    {
        protected override string GetTableName()
        {
            return "b_permit_role";
        }
        

        #region 搜索角色表

        /// <summary>
        ///  搜索角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<RoleMo>> SearchRoles(SearchReq req)
        {
            return SimpleSearch(req);
        }
        protected override string BuildSimpleSearchWhereSqlByFilterItem(string key, string value,
            Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "name":
                    return "t.name LIKE '%" + SqlFilter(value) + "%'";
            }

            return base.BuildSimpleSearchWhereSqlByFilterItem(key, value, sqlParas);
        }
        #endregion


        /// <summary>
        ///  通过角色Ids获取角色列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<ListResp<RoleMo>> GetList(IList<string> ids)
        {
            var sql = string.Concat("select * from ", TableName, " where id in(", string.Join(",", ids),
                ") and status>@status and owner_tid=@owner_tid");
            var paras = new { owner_tid = OwnerTId, status = (int)CommonStatus.Deleted };

            return GetList(sql, paras);
        }


        /// <summary>
        ///  通过角色id获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Task<Resp<RoleMo>> GetById(string id)
        {
            var cacheKey = string.Concat(CacheKeys.Perm_Role_ById, id);
            return CacheHelper.GetOrSetAsync(cacheKey, () => base.GetById(id), TimeSpan.FromHours(2),
                res => !res.IsSuccess());
        }

        /// <summary>
        /// 修改角色状态
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Task<Resp> UpdateStatus(string rid, CommonStatus status)
        {
            var cacheKey = string.Concat(CacheKeys.Perm_Role_ById, rid);

            return Update(u => new {u.status},
                u => u.id == rid && u.owner_tid == OwnerTId,
                new{ status })
                .WithCacheClear(cacheKey);
        }

        /// <summary>
        /// 修改角色名称
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Resp> UpdateName(string rid, string name)
        {
            var cacheKey = string.Concat(CacheKeys.Perm_Role_ById, rid);

            return Update(u => new { name },
                    u => u.id == rid && u.owner_tid == OwnerTId)
            .WithCacheClear(cacheKey);
        }


    }
}
