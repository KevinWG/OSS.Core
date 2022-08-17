using OSS.Common;
using OSS.Common.Encrypt;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  登录授权服务
    /// </summary>
    public class BaseAuthService 
    {
        /// <summary>
        ///  授权仓储（虚构表 - 物理层面在用户表中的特殊字段）
        /// </summary>
        protected static readonly IUserInfoRep m_AuthRep = InsContainer<IUserInfoRep>.Instance;
        
        #region 注册登录辅助方法
        
        /// <summary>
        ///  登录最终执行方法
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isSocialBind"></param>
        /// <returns></returns>
        protected static async Task<PortalTokenResp> LoginFinallyExecute(
            UserBasicMo user, bool isAdmin = false, int isSocialBind = 0)
        {
            if (!isAdmin && isSocialBind > 0)
            {
                var bindRes = await BindAuthSocialUserWithSysUser(user.id);
                if (!bindRes.IsSuccess())
                    return new PortalTokenResp().WithResp(bindRes);
            }

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
            var addRes = await RegAdd(user, isSocialBind);
            if (!addRes.IsSuccess())
                return new PortalTokenResp().WithResp(addRes);

            var identityRes = PortalIdentityHelper.GetRegLoginUserIdentity(user);
            return identityRes.IsSuccess()
                ? PortalTokenHelper.GeneratePortalToken(identityRes.data)
                : new PortalTokenResp().WithResp(identityRes);
        }


        protected static async Task<Resp<long>> RegAdd(UserInfoMo user, int isSocialBind)
        {
            if (!string.IsNullOrEmpty(user.email))
            {
                var checkEmailRes = await CheckNameIfCanReg(new PortalNameReq()
                {
                    type = PortalNameType.Email,
                    name = user.email
                });
                if (!checkEmailRes.IsSuccess())
                    return new LongResp().WithResp(checkEmailRes);
            }

            if (!string.IsNullOrEmpty(user.mobile))
            {
                var checkMobileRes = await CheckNameIfCanReg(new PortalNameReq()
                {
                    type = PortalNameType.Mobile,
                    name = user.mobile
                });
                if (!checkMobileRes.IsSuccess())
                    return new LongResp().WithResp(checkMobileRes);
            }

            user.FormatBaseByContext();

            if (isSocialBind > 0)
            {
                user.owner_uid = 0; //临时用户，格式化会被赋值，不需要

                var bindRes = await BindAuthSocialUserWithSysUser(user.id);
                if (!bindRes.IsSuccess())
                    return new LongResp().WithResp(bindRes);

                var socialUser = bindRes.data;
                user.nick_name = socialUser.nick_name;
                user.avatar    = socialUser.head_img;
            }

            if (string.IsNullOrEmpty(user.nick_name))
                user.nick_name = string.Concat("会员-", string.IsNullOrEmpty(user.mobile) ? user.email : user.mobile);

            await m_AuthRep.Add(user);

            return new LongResp(user.id);
        }

        protected static UserInfoMo GetRegisterUserInfo(string value, string passWord, PortalNameType type)
        {
            var userInfo = new UserInfoMo();
            userInfo.FormatBaseByContext();

            switch (type)
            {
                case PortalNameType.Mobile:
                    userInfo.mobile = value;
                    break;
                case PortalNameType.Email:
                    userInfo.email = value;
                    break;
                //case PortalNameType.ThirdOauth:
                //    userInfo.nick_name = value;
                //    break;
            }

            if (!string.IsNullOrEmpty(passWord))
                userInfo.pass_word = Md5.EncryptHexString(passWord);

            return userInfo;
        }

        protected static async Task<IResp> CheckNameIfCanReg(PortalNameReq req)
        {
            var userRes = await m_AuthRep.GetUserByLoginType(req.name, req.type);
            if (userRes.IsRespCode(RespCodes.OperateObjectNull))
                return new Resp();

            return userRes.IsSuccess()
                ? new Resp(RespCodes.OperateObjectExisted, "账号已存在，无法注册！")
                : new Resp().WithResp(userRes);
        }

        private static async Task<IResp<SocialUserMo>> BindAuthSocialUserWithSysUser(long userId)
        {
            var tokenDetailRes = PortalTokenHelper.FormatPortalToken();
            if (!tokenDetailRes.IsSuccess())
                return new Resp<SocialUserMo>().WithResp(tokenDetailRes);

            if (tokenDetailRes.data.authType != PortalAuthorizeType.SocialAppUser)
                return new Resp<SocialUserMo>().WithResp(RespCodes.OperateFailed, "第三方账号绑定失败！");

            var socialUserId = tokenDetailRes.data.userId;
            return await InsContainer<ISocialUserService>.Instance.BindWithSysUser(socialUserId, userId);
        }



        #endregion

        

    }
}