using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Portal.Domain;

namespace OSS.Core.Portal.Service.Common.Helpers;

internal static class PortalIdentityHelper
{
    #region 用户

    internal static async Task<Resp<UserIdentity>> GetUserIdentity(long userId)
    {
        var userRes = await InsContainer<IUserService>.Instance.GetUserById(userId);
        return !userRes.IsSuccess() 
            ? new Resp<UserIdentity>().WithResp(userRes, "获取用户信息异常!") 
            : GetRegLoginUserIdentity(userRes.data);
    }

    internal static Resp<UserIdentity> GetRegLoginUserIdentity(UserBasicMo user)
    {
        var checkRes = CheckIdentityStatus(user.status);
        if (!checkRes.IsSuccess())
            return new Resp<UserIdentity>().WithResp(checkRes);

        var identity = new UserIdentity
        {
            id     = user.id.ToString(),
            name   = user.nick_name ?? user.mobile ?? user.email,
            avatar = user.avatar,

            auth_type = PortalAuthorizeType.User
        };

        return new Resp<UserIdentity>(identity);
    }

    //  判断Identity 可用状态
    private static Resp CheckIdentityStatus(UserStatus state)
    {
        return state switch
        {
            UserStatus.Locked     => new Resp(RespTypes.UserBlocked, "账号已被锁定！"),
            UserStatus.WaitActive => new Resp(RespTypes.UserUnActive, "账号未激活！"),
            _                     => state < 0 ? new Resp(RespTypes.UserBlocked, "此账号异常！") : new Resp()
        };
    }

    #endregion

    #region 管理员

    internal static async Task<Resp<UserIdentity>> GetAdminIdentity(long userId)
    {
        var adminRes = await InsContainer<IAdminService>.Instance.GetAdminByUId(userId);
        if (!adminRes.IsSuccess())
            return new Resp<UserIdentity>().WithResp(adminRes, "管理员账号/密码错误!");

        var admin = adminRes.data;

        var checkRes = CheckIdentityStatus(admin.status);
        if (!checkRes.IsSuccess())
            return new Resp<UserIdentity>().WithResp(checkRes);

        var identity = new UserIdentity
        {
            id     = admin.id.ToString(), // 使用用户表的Id
            name   = admin.admin_name,
            avatar = admin.avatar,

            auth_type = admin.admin_type == AdminType.Supper
                ? PortalAuthorizeType.SuperAdmin
                : PortalAuthorizeType.Admin
        };
        return new Resp<UserIdentity>(identity);
    }

    //  判断Identity 可用状态     
    private static Resp CheckIdentityStatus(AdminStatus state)
    {
        return state < 0
            ? new Resp(RespTypes.UserBlocked, "此账号异常！")
            : new Resp();
    }

    #endregion

    
}