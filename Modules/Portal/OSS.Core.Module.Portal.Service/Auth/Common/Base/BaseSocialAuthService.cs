using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal
{
    public class BaseSocialAuthService : BaseAuthService
    {

        protected static readonly ISocialUserRep m_SocialRep = InsContainer<ISocialUserRep>.Instance;


        #region 第三方平台的登录注册辅助方法

        protected static Task<PortalTokenResp> RegLoginBySocialUser(SocialUserMo socialUser, bool is_admin)
        {
            return RegLoginBySocialUser(socialUser, is_admin, true);
        }

        protected static Task<PortalTokenResp> LoginBySocialUser(SocialUserMo socialUser, bool is_admin)
        {
            return RegLoginBySocialUser(socialUser, is_admin, false);
        }

        private static async Task<PortalTokenResp> RegLoginBySocialUser(SocialUserMo socialUser, bool is_admin, bool withRegTypeCheck)
        {
            var userId = socialUser.owner_uid;
            // = 已绑定系统用户的情况
            if (userId > 0)
            {
                var userRes = await InsContainer<IUserCommonService>.Instance.GetUserById(userId);
                if (!userRes.IsSuccess())
                    return new PortalTokenResp().WithResp(userRes, "第三方绑定账号信息异常!");

                return await LoginFinallyExecute(userRes.data, is_admin);
            }

            // = 非绑定系统用户下的情况
            // == 如果是管理员，登录失败
            if (is_admin)
                return new PortalTokenResp().WithResp(RespCodes.OperateFailed, "非管理员用户!");

            if (withRegTypeCheck)
            {
                // == 如果是用户，执行注册绑定机制
                var socialRegConfig = GetSocialRegConfig();
                if (socialRegConfig.reg_type == SocialRegisterType.JustRegister)
                {
                    var user = socialUser.ToUserInfo();
                    return await RegFinallyExecute(user, 1);
                }
            }

            var identity = PortalIdentityHelper.GetLoginSocialIdentity(socialUser);
            return PortalTokenHelper.GeneratePortalToken(identity);
        }


        /// <summary>
        ///     获取注册相关配置
        /// </summary>
        /// <para name="info">应用信息</para>
        /// <returns></returns>
        private static SocialRegisterConfig GetSocialRegConfig()
        {
            // todo 修改访问配置
            return new SocialRegisterConfig
            {
                reg_type = SocialRegisterType.UserBind
            };
        }

        #endregion
    }
}
