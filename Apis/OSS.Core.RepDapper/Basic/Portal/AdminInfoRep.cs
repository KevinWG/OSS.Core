#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 后台管理员仓储实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure;
using OSS.Core.RepDapper.Basic.Portal.Mos;

namespace OSS.Core.RepDapper.Basic.Portal
{
    /// <summary>
    ///  后台管理员仓储实现
    /// </summary> 
    public class AdminInfoRep : BaseRep<AdminInfoRep, AdminInfoMo>
    {
        protected override string GetTableName()
        {
            return "b_portal_admin";
        }

        /// <summary>
        ///   通过用户id获取管理员信息
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public Task<Resp<AdminInfoMo>> GetAdminByUId(long uId)
        {
            var adminKey = string.Concat(CoreCacheKeys.Portal_Admin_ByUId, uId);
            Func<Task<Resp<AdminInfoMo>>> getFunc = () => Get(a => a.u_id == uId);

            return getFunc.WithCache(adminKey, TimeSpan.FromHours(1));
        }



        /// <summary>
        ///  查询管理员列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<AdminInfoMo>> SearchAdmins(SearchReq req)
        {
            return SimpleSearch(req);
        }
        protected override string BuildSimpleSearchWhereSqlByFilterItem(string key, string value, Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "admin_name":
                    sqlParas.Add("@admin_name", value);
                    return "t.admin_name=@admin_name";
            }
            return base.BuildSimpleSearchWhereSqlByFilterItem(key, value, sqlParas);
        }



        /// <summary>
        ///  修改管理员状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adminStatus"></param>
        /// <returns></returns>
        public Task<Resp> UpdateStatus(long uId, AdminStatus adminStatus)
        {
            var adminKey = string.Concat(CoreCacheKeys.Portal_Admin_ByUId, uId);
            return Update(t => new { status = adminStatus }, t => t.u_id == uId )
                .WithCacheClear(adminKey);
        }

        /// <summary>
        ///   修改头像地址
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public Task<Resp> ChangeAvatar(long uId, string avatar)
        {
            var adminKey = string.Concat(CoreCacheKeys.Portal_Admin_ByUId, uId);

            return Update(u => new { avatar = avatar }, u => u.u_id == uId)
                .WithCacheClear(adminKey);
        }

        /// <summary>
        ///  修改管理员状态
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="adminType"></param>
        /// <returns></returns>
        public Task<Resp> SetAdminType(long uId, AdminType adminType)
        {
            var adminKey = string.Concat(CoreCacheKeys.Portal_Admin_ByUId, uId);
            return Update(t => new { admin_type = adminType }, t => t.u_id == uId )
                .WithCacheClear(adminKey);
        }

    }
}
