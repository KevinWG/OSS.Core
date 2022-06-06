﻿#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 后台管理员仓储实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Extension;
using OSS.Core.Portal.Domain;

namespace OSS.Core.Portal.Repository;

/// <summary>
///  后台管理员仓储实现
/// </summary> 
public class AdminInfoRep : BasePortalRep<AdminInfoRep, AdminInfoMo,long>, IAdminInfoRep
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
        var adminKey = string.Concat(PortalConst.CacheKeys.Portal_Admin_ByUId, uId);
        var getFunc = () => Get(a => a.id == uId);

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
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "admin_name":
                sqlParas.Add("@admin_name", value);
                return "t.admin_name=@admin_name";
        }
        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }



    /// <summary>
    ///  修改管理员状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="adminStatus"></param>
    /// <returns></returns>
    public Task<Resp> UpdateStatus(long uId, AdminStatus adminStatus)
    {
        var adminKey = string.Concat(PortalConst.CacheKeys.Portal_Admin_ByUId, uId);
        return Update(t => new { status = adminStatus }, t => t.id == uId)
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
        var adminKey = string.Concat(PortalConst.CacheKeys.Portal_Admin_ByUId, uId);

        return Update(u => new { avatar = avatar }, u => u.id == uId)
            .WithCacheClear(adminKey);
    }

    /// <summary>
    /// 修改管理员名称
    /// </summary>
    /// <param name="v"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<Resp> ChangeMyName(long uId, string name)
    {
        var adminKey = string.Concat(PortalConst.CacheKeys.Portal_Admin_ByUId, uId);

        return Update(u => new { admin_name = name }, u => u.id == uId)
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
        var adminKey = string.Concat(PortalConst.CacheKeys.Portal_Admin_ByUId, uId);
        return Update(t => new { admin_type = adminType }, t => t.id == uId)
            .WithCacheClear(adminKey);
    }

}