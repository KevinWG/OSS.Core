using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal
{
    internal static class PortalIdentityHelper
    {

        #region 用户

        internal static async Task<Resp<UserIdentity>> GetUserIdentity(long userId)
        {
            var userRes = await InsContainer<IUserCommonService>.Instance.GetUserById(userId);
            if (!userRes.IsSuccess())
                return new Resp<UserIdentity>().WithResp(userRes, "获取用户信息异常!");

            return GetRegLoginUserIdentity(userRes.data);
        }

        internal static Resp<UserIdentity> GetRegLoginUserIdentity(UserBasicMo user)
        {
            var checkRes = CheckIdentityStatus(user.status);
            if (!checkRes.IsSuccess())
                return new Resp<UserIdentity>().WithResp(checkRes);

            var identity = new UserIdentity
            {
                id = user.id.ToString(),
                name = user.nick_name ?? user.mobile ?? user.email,
                avatar = user.avatar,
                //from_plat = (int)fromPlat,

                auth_type = PortalAuthorizeType.User
            };

            return new Resp<UserIdentity>(identity);
        }

        //  判断Identity 可用状态
        private static Resp CheckIdentityStatus(UserStatus state)
        {
            return state switch
            {
                UserStatus.Locked     => new Resp(RespCodes.UserBlocked, "账号已被锁定！"),
                UserStatus.WaitActive => new Resp(RespCodes.UserUnActive, "账号未激活！"),

                _                     => state < 0 ? new Resp(RespCodes.UserBlocked, "此账号异常！") : new Resp()
            };
        }

        #endregion

        #region 管理员

        internal static async Task<Resp<UserIdentity>> GetAdminIdentity(long userId)
        {
            var adminRes = await InsContainer<IAdminCommonService>.Instance.GetAdminByUId(userId);
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
                //from_plat = (int)fromPlat,

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
                ? new Resp(RespCodes.UserBlocked, "此账号异常！")
                : new Resp();
        }

        #endregion

        #region 第三方社交平台

        internal static async Task<Resp<UserIdentity>> GetSocialIdentity(long socialUserId)
        {
            var socialRes = await InsContainer<ISocialUserCommonService>.Instance.GetById(socialUserId);
            if (!socialRes.IsSuccess())
                return new Resp<UserIdentity>().WithResp(socialRes, "获取第三方信息异常!");

            return new Resp<UserIdentity>(GetLoginSocialIdentity(socialRes.data));
        }

        internal static UserIdentity GetLoginSocialIdentity(SocialUserMo socialUser)
        {
            // 当前是临时第三方信息，【Id】是第三方授权表的oauthId
            //  此场景是给用户授权后选择是否绑定已有账户页面使用
            return new UserIdentity
            {
                id = socialUser.id.ToString(),
                name = socialUser.nick_name,
                avatar = socialUser.head_img,
                //from_plat = (int)socialUser.social_plat,

                auth_type = PortalAuthorizeType.SocialAppUser,
            };
        }

        #endregion
    }
}
