using OSS.Common;
using OSS.Common.Encrypt;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Service.Common.Helpers;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Service;

namespace OSS.Core.Portal.Service
{
    /// <summary>
    ///  登录授权服务
    /// </summary>
    public class BaseAuthService : BaseService
    {
        // 当前Service管理 user 表特定登录字段在，后期可拆分独立表处理
        protected static readonly IUserInfoRep m_PortalRep = InsContainer<IUserInfoRep>.Instance;
        
        #region 注册登录辅助方法

        /// <summary>
        ///  登录最终执行方法
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isSocialBind"></param>
        /// <returns></returns>
        protected static async Task<PortalTokenResp> LoginFinallyExecute(UserInfoMo user, bool isAdmin = false, int isSocialBind=0)
        {
            //if (!isAdmin&&isSocialBind > 0)
            //{
            //    var bindRes = await BindAuthSocialUserWithSysUser(user.id);
            //    if (!bindRes.IsSuccess())
            //        return new PortalTokenResp().WithResp(bindRes);
            //}

            var identityRes = isAdmin
                ? await PortalIdentityHelper.GetAdminIdentity(user.id)
                : PortalIdentityHelper.GetRegLoginUserIdentity(user);

            if (!identityRes.IsSuccess())
                return new PortalTokenResp().WithResp(identityRes);
            
            return PortalTokenHelper.GeneratePortalToken(identityRes.data);
        }

        /// <summary>
        ///  注册最终执行方法
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isSocialBind"></param>
        /// <returns></returns>
        protected static async Task<PortalTokenResp> RegFinallyExecute(UserInfoMo user, int isSocialBind)
        {
            user.FormatBaseByContext();

            user.owner_uid = 0; // 防止第三方账号临时登录污染

            //if (isSocialBind >0)
            //{
            //    var bindRes = await BindAuthSocialUserWithSysUser(user.id);
            //    if (!bindRes.IsSuccess())
            //        return new PortalTokenResp().WithResp(bindRes);

            //    var socialUser = bindRes.data;
            //    user.nick_name = socialUser.nick_name;
            //    user.avatar    = socialUser.head_img;
            //}

            if (string.IsNullOrEmpty(user.nick_name))
                user.nick_name = string.Concat("会员-", user.mobile ?? user.email);

            var idRes = await m_PortalRep.Add(user);
            if (!idRes.IsSuccess())
                return new PortalTokenResp().WithResp(idRes, "创建注册用户失败!");

            var identityRes = PortalIdentityHelper.GetRegLoginUserIdentity(user);
            return identityRes.IsSuccess()
                ? PortalTokenHelper.GeneratePortalToken(identityRes.data)
                : new PortalTokenResp().WithResp(identityRes);
        }

        protected static UserInfoMo GetRegisterUserInfo(string? value, string? passWord, PortalType type)
        {
            var userInfo = new UserInfoMo();

            userInfo.FormatBaseByContext();

            switch (type)
            {
                case PortalType.Mobile:
                    userInfo.mobile = value;
                    break;
                case PortalType.Email:
                    userInfo.email = value;
                    break;
                case PortalType.ThirdOauth:
                    userInfo.nick_name = value;
                    break;
            }

            if (!string.IsNullOrEmpty(passWord))
                userInfo.pass_word = Md5.EncryptHexString(passWord);

            return userInfo;
        }
        
        #endregion
    }
}