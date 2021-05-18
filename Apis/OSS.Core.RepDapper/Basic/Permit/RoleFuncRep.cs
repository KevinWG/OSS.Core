using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Enums;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Extensions;
using OSS.Core.RepDapper.Basic.Permit.Mos;

namespace OSS.Core.RepDapper.Basic.Permit
{
    public class RoleFuncRep : BaseRep<RoleFuncRep, RoleFuncMo>
    {
        protected override string GetTableName()
        {
            return "b_permit_role_func";
        }

        /// <summary>
        ///  获取指定角色的权限列表
        /// </summary>
        /// <returns></returns>
        public Task<ListResp<RoleFunSmallMo>> GetRoleFuncList(long roleId)
        {
            var key = string.Concat(CacheKeys.Perm_RoleFuncs_ByRId, roleId);
            var sql = string.Concat("select * from ", TableName, " where role_id=@role_id and status>@status ");

            Func<Task<ListResp<RoleFunSmallMo>>> getFunc = () =>
                GetList<RoleFunSmallMo>(sql, new {role_id = roleId, status = CommonStatus.Deleted});

          return  getFunc.WithCache(key, TimeSpan.FromHours(2));
        }

        /// <summary>
        ///  获取多个角色的权限列表
        /// </summary>
        /// <returns></returns> 
        public Task<ListResp<RoleFunSmallMo>> GetRoleFuncList(List<long> roleIds)
        {
            var sql = string.Concat("select func_code,data_level from ", TableName, " where role_id in (",
                SqlFilter(string.Join(",",roleIds)),
                ") and status>@status");

            return GetList<RoleFunSmallMo>(sql, new {status = CommonStatus.Deleted});
        }

        /// <summary>
        ///  修改关联的权限项
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="addItems"></param>
        /// <param name="deleteItems"></param>
        /// <returns></returns>
        public async Task<Resp> ChangeRoleFuncItems(long rid, List<RoleFuncMo> addItems, List<string> deleteItems)
        {
            var key = string.Concat(CacheKeys.Perm_RoleFuncs_ByRId, rid);
            if (deleteItems?.Count>0)
            {
                var softDeleteWhereSql =
                    " role_id=@role_id and status>@status and func_code in ('" +
                    SqlFilter(string.Join(",", deleteItems)).Replace(",", "','") + "')";

                var sdRes = await SoftDelete(softDeleteWhereSql,
                    new { status = CommonStatus.Deleted, role_id = rid }).WithCacheClear(key);

                if (!sdRes.IsSuccess() || !(addItems?.Count > 0))
                    return sdRes;
            }
           
            var addRes = await AddList(addItems).WithCacheClear(key); 
            return addRes.IsSuccess() ? addRes : new Resp(addRes.ret, "新增部分失败！");
        }
    }
}
