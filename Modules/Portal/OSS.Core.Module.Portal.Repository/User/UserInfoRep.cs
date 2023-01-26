#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 后台用户仓储实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal;

public class UserInfoRep : BasePortalRep<UserInfoMo>, IUserInfoRep
{

    public UserInfoRep() : base("b_portal_user")
    {
    }

    
    /// <summary>
    ///  获取平台列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public Task<PageList<UserBasicMo>> SearchUsers(SearchReq search)
    {
        return SimpleSearch<UserBasicMo>(search);
    }


    protected override string BuildSimpleSearch_FilterItemSql(string key, string value,
                                                              Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "email":
                sqlParas.Add("@email", value);
                return "email=@email";
            case "mobile":
                sqlParas.Add("@mobile", value);
                return "mobile=@mobile";

            case "reg_start_at":
                sqlParas.Add("@reg_start_at", value.ToInt64());
                return "add_time>=@reg_start_at";
            case "reg_end_at":
                sqlParas.Add("@reg_end_at", value.ToInt64());
                return "add_time<=@reg_end_at";
        }

        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }

    /// <summary>
    ///  获取用户信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<Resp<UserInfoMo>> GetUserByLoginType(string name, PortalNameType type)
    {
        return type == PortalNameType.Mobile
            ? Get(u => u.mobile == name && u.status >= UserStatus.Locked)
            : Get(u => u.email == name && u.status >= UserStatus.Locked);
    }




    /// <summary>
    ///  修改用户登录信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<Resp> UpdatePortalByType(long id, PortalNameType type, string name)
    {
        var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);
        return (
            type == PortalNameType.Mobile
                ? Update(t => new {mobile = name}, t => t.id == id)
                : Update(t => new {email  = name}, t => t.id == id)
        ).WithRespCacheClearAsync(userKey);
    }


    public override Task<Resp<UserInfoMo>> GetById(long id)
    {
        var getFunc = () => base.GetById(id);

        var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);

        return getFunc.WithRespCacheAsync(userKey, TimeSpan.FromHours(1));
    }


    /// <summary>
    ///  修改用户状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public Task<Resp> UpdateStatus(long id, UserStatus state)
    {
        var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);
        return Update(t => new {status = state}, t => t.id == id)
            .WithRespCacheClearAsync(userKey);
    }


    /// <summary>
    ///  修改基础信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="avatar"></param>
    /// <param name="nickName"></param>
    /// <returns></returns>
    public Task<Resp> UpdateBasicInfo(long id, string avatar, string nickName)
    {
        var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);
        return Update(t => new {avatar = avatar, nick_name = nickName}, t => t.id == id)
            .WithRespCacheClearAsync(userKey);
    }

}