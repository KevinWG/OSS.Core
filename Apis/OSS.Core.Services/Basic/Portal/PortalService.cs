#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 登录注册入口 service （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.Portal;
using OSS.Core.RepDapper.Basic.Portal.Mos; 
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;
using OSS.Core.Services.Basic.Portal.Events;
using OSS.Core.Services.Basic.Portal.IProxies;
using OSS.Core.Services.Basic.Portal.Mos;
using OSS.Tools.Config;
using OSS.Core.Infrastructure.Const;
using System;
using OSS.Core.Infrastructure;
using OSS.Core.RepDapper;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class PortalService : IPortalServiceProxy
    {
        /// <summary>
        ///     检查账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<Resp> CheckIfCanReg(RegLoginType type, string value)
        {
            var userRes = await UserInfoRep.Instance.GetUserByLoginType(value, type);

            if (userRes.IsRespType(RespTypes.OperateObjectNull))
            {
                return new Resp();
            }
          
            return userRes.IsSuccess()
                ? new Resp(RespTypes.OperateObjectExist, "账号已存在，无法注册！")
                : new Resp().WithResp(userRes);
        }

        /// <summary>
        ///  获取授权账号信息
        /// </summary>
        /// <returns></returns>
        public Task<Resp<UserIdentity>> GetMyself()
        {
            var cacheKey = string.Concat(CoreCacheKeys.Portal_UserIdentity_ByToken, CoreAppContext.Identity.token);
            Func<Task<Resp<UserIdentity>>> getFunc = () =>
            {
                var infoRes = FormatPortalToken();
                if (!infoRes.IsSuccess())
                    return Task.FromResult(new Resp<UserIdentity>().WithResp(infoRes));

                return GetAuthIdentityById(infoRes.data.userId, infoRes.data.authType, infoRes.data.plat);
            };
            return getFunc.WithAbsoluteCache(cacheKey, TimeSpan.FromMinutes(5));
        }

        #region 注册登录辅助方法

        private async Task<PortalTokenResp> LoginFinallyExecute(UserInfoBigMo user,
            PortalAuthorizeType authType, bool isFromThirdBind = false, SocialPlatform plat = SocialPlatform.None)
        {
            Resp<UserIdentity> identityRes;
            switch (authType)
            {
                case PortalAuthorizeType.Admin: //  登录时默认只有admin，不会出现 superAdmin

                    var adminRes = await AdminInfoRep.Instance.GetAdminByUId(user.id);
                    if (!adminRes.IsSuccess())
                        return new PortalTokenResp() {ret = adminRes.ret, msg = "管理员账号/密码错误!"};

                    identityRes = InitialAdminIdentity(adminRes.data,plat);
                    break;

                case PortalAuthorizeType.User:
                    identityRes = InitialUserIdentity(user,plat);
                    break;

                default:
                    return new PortalTokenResp(RespTypes.ParaError, "账号/密码错误！");
            }

            if (!identityRes.IsSuccess())
                return new PortalTokenResp().WithResp(identityRes);

            if (isFromThirdBind)
            {
                var bindRes = await BindTempOauthUserToUser(user.id);
                if (!bindRes.IsSuccess())
                    return new PortalTokenResp().WithResp(bindRes);

                plat = bindRes.data;
            }

            PortalEvents.TriggerLoginEvent(identityRes.data, CoreAppContext.Identity);
            return GeneratePortalToken(identityRes.data, plat);
        }

        private async Task<PortalTokenResp> RegFinallyExecute(UserInfoBigMo user, PortalAuthorizeType authType,
            bool isFromBind = false, SocialPlatform plat = SocialPlatform.None)
        {
            var idRes = await UserInfoRep.Instance.Add(user);
            if (!idRes.IsSuccess())
                return new PortalTokenResp().WithResp(idRes);

            if (isFromBind)
            {
                var bindRes = await BindTempOauthUserToUser(user.id);
                if (!bindRes.IsSuccess())
                    return new PortalTokenResp().WithResp(bindRes); //bindRes;

                plat = bindRes.data;
            }

            user.pass_word = null;
            var identity = new UserIdentity()
            {
                auth_type = authType,

                id     = user.id.ToString(),
                name   = user.nick_name,
                avatar = user.avatar
            };

            PortalEvents.TriggerRegisterEvent(identity, plat, CoreAppContext.Identity);
            return GeneratePortalToken(identity, plat);
            ;
        }

        private static UserInfoBigMo GetRegisterUserInfo(string value, string passWord, RegLoginType type)
        {
            var sysInfo = CoreAppContext.Identity;
            var userInfo = new UserInfoBigMo();

            userInfo.InitialBaseFromContext();

            switch (type)
            {
                case RegLoginType.Mobile:
                    userInfo.mobile = value;
                    break;
                case RegLoginType.Email:
                    userInfo.email = value;
                    break;
                case RegLoginType.ThirdName:
                    userInfo.nick_name = value;
                    break;
            }

            if (!string.IsNullOrEmpty(passWord))
                userInfo.pass_word = Md5.EncryptHexString(passWord);

            return userInfo;
        }


        private async Task<Resp<SocialPlatform>> BindTempOauthUserToUser(long userId)
        {
            var tokenDetailRes = FormatPortalToken();
            if (!tokenDetailRes.IsSuccess())
                return new Resp<SocialPlatform>().WithResp(tokenDetailRes);

            if (tokenDetailRes.data.authType != PortalAuthorizeType.SocialAppUser)
                return new Resp<SocialPlatform>().WithResp(RespTypes.OperateFailed, "未发现第三方临时授权信息！");

            var oauthUserId = tokenDetailRes.data.userId;
            var bindRes     = await OauthUserRep.Instance.BindUserIdByOauthId(oauthUserId, userId);
            if (!bindRes.IsSuccess())
            {
                return new Resp<SocialPlatform>().WithResp(bindRes);
            }

            return new Resp<SocialPlatform>(tokenDetailRes.data.plat);
        }

        #endregion

        #region 通过授权类型和Id获取授权信息

        private static async Task<Resp<UserIdentity>> GetAuthIdentityById(long userId,
            PortalAuthorizeType authType, SocialPlatform fromPlat)
        {
            Resp<UserIdentity> identityRes;
            switch (authType)
            {
                case PortalAuthorizeType.Admin:
                case PortalAuthorizeType.SuperAdmin:
                    var adminRes = await AdminInfoRep.Instance.GetAdminByUId(userId);
                    if (!adminRes.IsSuccess())
                        return new Resp<UserIdentity>().WithResp(RespTypes.UserUnLogin, "用户未登录!");

                    identityRes = InitialAdminIdentity(adminRes.data, fromPlat);
                    break;
                case PortalAuthorizeType.User:
                    var userRes = await UserInfoRep.Instance.GetById(userId);
                    if (!userRes.IsSuccess())
                        return new Resp<UserIdentity>().WithResp(RespTypes.UserUnLogin, "用户未登录!");

                    identityRes = InitialUserIdentity(userRes.data, fromPlat);
                    break;
                default:
                    identityRes = new Resp<UserIdentity>().WithResp(RespTypes.UserUnLogin, "用户未登录!");
                    break;
            }

            return identityRes;
        }

        private static Resp<UserIdentity> InitialUserIdentity(UserInfoBigMo user, SocialPlatform fromPlat)
        {
            user.pass_word = null; //  不可传出

            var checkRes = CheckIdentityStatus(user.status);
            if (!checkRes.IsSuccess())
                return new Resp<UserIdentity>().WithResp(checkRes);

            var identity = new UserIdentity
            {
                id        = user.id.ToString(),
                name      = user.nick_name??user.mobile??user.email,
                avatar    = user.avatar,
                //from_plat = (int) fromPlat,

                auth_type = PortalAuthorizeType.User
            };

            return new Resp<UserIdentity>(identity);
        }

        private static Resp<UserIdentity> InitialAdminIdentity(AdminInfoMo admin, SocialPlatform fromPlat)
        {
            var checkRes = CheckIdentityStatus(admin.status);
            if (!checkRes.IsSuccess())
                return new Resp<UserIdentity>().WithResp(checkRes);

            var identity = new UserIdentity
            {
                id        = admin.u_id.ToString(), // 使用用户表的Id
                name      = admin.admin_name,
                avatar    = admin.avatar,
                //from_plat = (int) fromPlat,

                auth_type = admin.admin_type == AdminType.Supper
                    ? PortalAuthorizeType.SuperAdmin
                    : PortalAuthorizeType.Admin
            };
            return new Resp<UserIdentity>(identity);
        }

        private static Resp<UserIdentity> InitialOauthTempIdentity(OauthUserMo oauthUser, SocialPlatform fromPlat)
        {
            var checkRes = CheckIdentityStatus(oauthUser.status);
            if (!checkRes.IsSuccess())
                return new Resp<UserIdentity>().WithResp(checkRes);

            // 当前是临时第三方信息，【Id】是第三方授权表的oauthId
            //  此场景是给用户授权后选择是否绑定已有账户页面使用
            var identity = new UserIdentity
            {
                id        = oauthUser.id.ToString(),
                name      = oauthUser.nick_name,
                avatar    = oauthUser.head_img,
                //from_plat = (int) fromPlat,

                auth_type = PortalAuthorizeType.SocialAppUser,
            };

            return new Resp<UserIdentity>(identity);
        }


        //  判断Identity 可用状态     
        private static Resp CheckIdentityStatus(UserStatus state)
        {
            return state < 0
                ? new Resp().WithResp(RespTypes.UserUnActive, "此账号异常！")
                : new Resp();
        }

        //  判断Identity 可用状态     
        public static Resp CheckIdentityStatus(AdminStatus state)
        {
            return state < 0
                ? new Resp().WithResp(RespTypes.UserUnActive, "此账号异常！")
                : new Resp();
        }


        #endregion

        #region 生成/解析 用户授权Token

        /// <summary>
        ///  应用内部Token加密秘钥
        /// </summary>
        private static readonly string _portalTokenSecret =
            ConfigHelper.GetSection("SiteConfig:PortalTokenSecret")?.Value;


        /// <summary>
        ///  获取授权token相关授权信息
        ///    todo 扩展每个AppId独立的加密秘钥
        /// </summary>
        /// <param name="newIdentity"></param>
        /// <param name="plat"></param>
        /// <returns></returns>
        private PortalTokenResp GeneratePortalToken(UserIdentity newIdentity, SocialPlatform plat)
        {
            var tenantId = CoreAppContext.Identity.tenant_id;
            var tokenStr = string.Concat(newIdentity.id, "|", tenantId, "|", (int)newIdentity.auth_type, "|", (int)plat, "|",
                NumHelper.RandomNum(6));

            var token = CoreUserContext.GetToken(_portalTokenSecret, tokenStr);
            return new PortalTokenResp {token = token, data = newIdentity};
        }


        private static Resp<(long userId, PortalAuthorizeType authType, SocialPlatform plat)> FormatPortalToken()
        {
            var appInfo = CoreAppContext.Identity;
            if (string.IsNullOrEmpty(appInfo.token))
                return new Resp<(long, PortalAuthorizeType, SocialPlatform plat)>().WithResp(RespTypes.UserUnLogin,
                    "用户未登录");

            var tokenDetail = CoreUserContext.GetTokenDetail(_portalTokenSecret, appInfo.token);
            var tokenSplit = tokenDetail.Split('|');
            if (tokenSplit.Length != 5)
                return new Resp<(long, PortalAuthorizeType, SocialPlatform plat)>().WithResp(RespTypes.OperateFailed,
                    "非合法授权来源!");

            var tenantId = tokenSplit[1];
            var userId   = tokenSplit[0].ToInt64();

            if ( !string.IsNullOrEmpty(appInfo.tenant_id) && tenantId != appInfo.tenant_id 
                || userId<=0)
                return new Resp<(long, PortalAuthorizeType, SocialPlatform plat)>().WithResp(RespTypes.OperateFailed,
                    "非合法授权来源!");

            var authType = (PortalAuthorizeType) tokenSplit[2].ToInt32();
            var plat     = (SocialPlatform) tokenSplit[3].ToInt32();

            return new Resp<(long, PortalAuthorizeType, SocialPlatform plat)>()
            {
                data = (userId, authType, plat)
            };
        }

        #endregion
    }
}