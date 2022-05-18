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

using OSS.Common.Resp;
using OSS.Common.Encrypt;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Common;
using OSS.Tools.Cache;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Core.Common.Const;
using OSS.Core.Reps.Basic.Portal;
using OSS.Core.Reps.Basic.Portal.Mos;
using OSS.Core.Services.Basic.Portal.Helpers;
using OSS.Core.Services.Basic.Portal.IProxies;
using OSS.Core.Services.Basic.Portal.Reqs;
using OSS.Core.Services.Plugs.Notify;
using OSS.Core.Services.Plugs.Notify.Mos;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class PortalService : BasePortalService, IPortalServiceProxy
    {
        #region 获取登录认证信息

        /// <summary>
        ///  获取授权账号信息
        /// </summary>
        /// <returns></returns>
        public Task<Resp<UserIdentity>> GetIdentity()
        {
            var cacheKey = string.Concat(CacheKeys.Portal_UserIdentity_ByToken, CoreAppContext.Identity.token);
            Func<Task<Resp<UserIdentity>>> getFunc = () =>
            {
                var infoRes = PortalTokenHelper.FormatPortalToken();
                if (!infoRes.IsSuccess())
                    return Task.FromResult(new Resp<UserIdentity>().WithResp(infoRes));

                return GetAuthIdentityById(infoRes.data.userId, infoRes.data.authType);
            };
            return getFunc.WithAbsoluteCache(cacheKey, TimeSpan.FromMinutes(5));
        }

        private static async Task<Resp<UserIdentity>> GetAuthIdentityById(long userId, PortalAuthorizeType authType)
        {
            Resp<UserIdentity> identityRes;
            switch (authType)
            {
                case PortalAuthorizeType.Admin:
                case PortalAuthorizeType.SuperAdmin:
                    identityRes = await PortalIdentityHelper.GetAdminIdentity(userId);
                    break;

                case PortalAuthorizeType.User:
                case PortalAuthorizeType.UserWithEmpty:
                    identityRes = await PortalIdentityHelper.GetUserIdentity(userId);
                    break;

                case PortalAuthorizeType.SocialAppUser:
                    identityRes = await PortalIdentityHelper.GetSocialIdentity(userId);
                    break;
                default:
                    identityRes = new Resp<UserIdentity>().WithResp(RespTypes.UserUnLogin, "用户未登录!");
                    break;
            }

            if (!identityRes.IsSuccess() && !identityRes.IsRespType(RespTypes.UserBlocked) && !identityRes.IsRespType(RespTypes.UserUnActive))
            {
                // 正常请求获取登录信息时异常，统一返回未登录错误码
                identityRes.WithResp(RespTypes.UserUnLogin, identityRes.msg);
            }
            return identityRes;
        }

        #endregion

        #region 验证是否可以注册

        /// <summary>
        ///     检查账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<Resp> CheckIfCanReg(PortalNameReq req)
        {
            var userRes = await UserInfoRep.Instance.GetUserByLoginType(req.name, req.type);
            if (userRes.IsRespType(RespTypes.OperateObjectNull))
                return new Resp();

            return userRes.IsSuccess()
                ? new Resp(RespTypes.OperateObjectExist, "账号已存在，无法注册！")
                : new Resp().WithResp(userRes);
        }

        #endregion

        #region 动态验证码登录注册实现

        /// <summary>
        ///     用户动态码登录
        /// </summary>
        /// <returns></returns>
        public Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req)
        {
            return CodeLogin(req, false);
        }

        /// <summary>
        /// 管理员动态码登录
        /// </summary>
        /// <returns></returns>
        public Task<PortalTokenResp> CodeAdminLogin(PortalPasscodeReq req)
        {
            return CodeLogin(req, true);
        }


        private static readonly PortalTokenResp _codeLoginError = new PortalTokenResp(RespTypes.ParaError, "手机号未注册或验证码错误！");

        /// <summary>
        ///   用户动态码登录注册（如果不存在则直接注册
        /// </summary>
        /// <returns></returns>
        public async Task<PortalTokenResp> CodeRegOrLogin(PortalPasscodeReq req)
        {
            var codeRes = await CheckPasscode(req.name, req.code);
            if (!codeRes.IsSuccess())
                return new PortalTokenResp().WithResp(codeRes);

            var userRes = await UserInfoRep.Instance.GetUserByLoginType(req.name, req.type);
            if (userRes.IsSuccess())
                return await LoginFinallyExecute(userRes.data,false, req.is_social_bind);

            if (!userRes.IsRespType(RespTypes.OperateObjectNull))
                return _codeLoginError;

            var checkRes = await CheckIfCanReg(req);
            if (!checkRes.IsSuccess()) return new PortalTokenResp().WithResp(checkRes);

            // 执行注册
            var userInfo = GetRegisterUserInfo(req.name, string.Empty, req.type.ToPortalType());
            return await RegFinallyExecute(userInfo, req.is_social_bind);
        }

        //  动态码验证登录
        private async Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req, bool isAdmin)
        {
            var codeRes = await CheckPasscode(req.name, req.code);
            if (!codeRes.IsSuccess())
                return new PortalTokenResp().WithResp(codeRes);

            var userRes = await UserInfoRep.Instance.GetUserByLoginType(req.name, req.type);

            return userRes.IsSuccess()
                ? await LoginFinallyExecute(userRes.data, isAdmin, req.is_social_bind)
                : _codeLoginError;
        }




        /// <summary>
        ///     发送动态码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Resp> SendCode(PortalLoginBaseReq req)
        {
            var code = NumHelper.RandomNum();
            var notifyMsg = new NotifyReq
            {
                targets = new List<string>() { req.name },
                body_paras = new Dictionary<string, string> { { "code", code } },

                t_code = req.type == PortalCodeType.Mobile
                    ? DirConfigKeys.plugs_notify_sms_portal_tcode
                    : DirConfigKeys.plugs_notify_email_portal_tcode
            };

            var res = await OSS.Common.InsContainer<INotifyService>.Instance.Send(notifyMsg);
            if (!res.IsSuccess())
                return res;

            var key = string.Concat(CacheKeys.Portal_Passcode_ByLoginName, req.name);
            await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));

            return res;
        }

        /// <summary>
        ///     验证动态码是否正确
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        private static async Task<Resp> CheckPasscode(string loginName, string passcode)
        {
            var key = string.Concat(CacheKeys.Portal_Passcode_ByLoginName, loginName);
            var code = await CacheHelper.GetAsync<string>(key);

            if (string.IsNullOrEmpty(code) || passcode != code)
                return new Resp(RespTypes.OperateFailed, "验证码错误");

            await CacheHelper.RemoveAsync(key);
            return new Resp();
        }

        #endregion

        #region 账号密码登录注册实现

        /// <summary>
        ///   直接通过账号密码注册用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PortalTokenResp> PwdReg(PortalPasswordReq req)
        {
            var checkRes = await CheckIfCanReg(req);
            if (!checkRes.IsSuccess()) return new PortalTokenResp().WithResp(checkRes); // checkRes.ConvertToResultInherit<PortalTokenResp>();

            var userInfo = GetRegisterUserInfo(req.name, req.password, req.type.ToPortalType());

            userInfo.status = UserStatus.WaitActive;//  默认待激活状态
            return await RegFinallyExecute(userInfo, req.is_social_bind);
        }

        /// <summary> 
        ///     用户密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PortalTokenResp> PwdLogin(PortalPasswordReq req)
        {
            return PwdLogin(req, false);
        }

        /// <summary>
        ///     管理员用户密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PortalTokenResp> PwdAdminLogin(PortalPasswordReq req)
        {
            return PwdLogin(req, true);
        }

        private async Task<PortalTokenResp> PwdLogin(PortalPasswordReq req, bool isAdmin)
        {
            var userRes = await UserInfoRep.Instance.GetUserByLoginType(req.name, req.type);
            if (userRes.IsSuccess())
            {
                var user = userRes.data;
                if (Md5.EncryptHexString(req.password) == user.pass_word)
                {
                    return await LoginFinallyExecute(user, isAdmin, req.is_social_bind);
                }
            }
            return new PortalTokenResp(RespTypes.ParaError, "账号密码不正确！");
        }


        #endregion
    }
}