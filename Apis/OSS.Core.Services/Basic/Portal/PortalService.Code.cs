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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Services.Basic.Portal.Mos;
using OSS.Core.RepDapper.Basic.Portal;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Tools.Cache;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.RepDapper.Basic.Portal.Mos;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class PortalService
    {
        #region  动态验证码登录或注册

        /// <summary>
        ///   动态码注册
        /// </summary>
        /// <returns></returns>
        public async Task<PortalTokenResp> CodeReg(string name ,string code,RegLoginType type,bool isFromThirdBind)
        {
            var codeRes =await CheckPasscode(name, code);
            if (!codeRes.IsSuccess())
                return new PortalTokenResp().WithResp(codeRes);
            
            var checkRes = await CheckIfCanReg(type, name);
            if (!checkRes.IsSuccess()) return new PortalTokenResp().WithResp(codeRes);

            // 执行注册
            var userInfo = GetRegisterUserInfo(name, string.Empty, type);
            return await RegFinallyExecute(userInfo, PortalAuthorizeType.User,isFromThirdBind);
        }
        /// <summary>
        ///     用户动态码登录
        /// </summary>
        /// <returns></returns>
        public Task<PortalTokenResp> CodeLogin(string name, string code, RegLoginType type, bool isFromThirdBind = false)
        {
            return CodeLogin(name, code, type,  PortalAuthorizeType.User, isFromThirdBind);
        }

        /// <summary>
        /// 管理员动态码登录
        /// </summary>
        /// <returns></returns>
        public Task<PortalTokenResp> CodeAdminLogin(string name, string code, RegLoginType type, bool isFromThirdBind = false)
        {
            return CodeLogin(name, code, type, PortalAuthorizeType.Admin, isFromThirdBind);
        }

        //  动态码验证登录
        private async Task<PortalTokenResp> CodeLogin(string name,string code,RegLoginType type,
            PortalAuthorizeType authType, bool isFromThirdBind = false)
        {
            var codeRes =await CheckPasscode(name, code);
            if (!codeRes.IsSuccess())
                return new PortalTokenResp().WithResp(codeRes);

            var userRes = await UserInfoRep.Instance.GetUserByLoginType(name, type);
            if (!userRes.IsSuccess())
                return new PortalTokenResp() {ret = userRes.ret, msg = "账号或密码错误！"};

            // 执行登录
            return await LoginFinallyExecute(userRes.data, authType, isFromThirdBind, SocialPlatform.None);
        }

        /// <summary>
        ///     动态码注册
        /// </summary>
        /// <returns></returns>
        public async Task<PortalTokenResp> CodeRegOrLogin(string name, string code, RegLoginType type, bool isFromThirdBind)
        {
            var codeRes =await CheckPasscode(name, code);
            if (!codeRes.IsSuccess())
                return new PortalTokenResp().WithResp(codeRes);

            var userRes = await UserInfoRep.Instance.GetUserByLoginType(name, type);
            if (userRes.IsSuccess())
                return await LoginFinallyExecute(userRes.data, PortalAuthorizeType.User, isFromThirdBind);

            if (!userRes.IsRespType(RespTypes.ObjectNull))
                return new PortalTokenResp() {ret = userRes.ret, msg = "账号密码错误！"};

            // 执行注册
            var userInfo = GetRegisterUserInfo(name, string.Empty, type);
            return await RegFinallyExecute(userInfo, PortalAuthorizeType.User, isFromThirdBind);
        }

        #endregion

        /// <summary>
        ///     发送动态码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Resp> SendCode(string loginName, RegLoginType type)
        {
            var code = NumHelper.RandomNum();

            var notifyMsg = new NotifyReq
            {
                targets    = new List<string>() {loginName},
                body_paras = new Dictionary<string, string> {{"code", code}},

                t_code     = type == RegLoginType.Mobile 
                ? CoreDirConfigKeys.plugs_notify_sms_portal_tcode 
                : CoreDirConfigKeys.plugs_notify_email_portal_tcode
            };

            var res = await InsContainer<INotifyServiceProxy>.Instance.Send(notifyMsg);
            if (!res.IsSuccess())
                return res ?? new Resp().WithResp(RespTypes.UnKnowSource, "未知类型！");

            var key = string.Concat(CoreCacheKeys.Portal_Passcode_ByLoginName, loginName);
            await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));
            return res;
        }

        #region 辅助方法

        /// <summary>
        ///     验证动态码是否正确
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        private static async Task<Resp> CheckPasscode(string loginName, string passcode)
        {
            if (AppInfoHelper.IsDev && passcode == "1111")
            {
                return new Resp();
            }

#if DEBUG
            if (AppInfoHelper.IsDev && passcode=="1111")
            {
                return new Resp();
            }
#endif
            var key = string.Concat(CoreCacheKeys.Portal_Passcode_ByLoginName, loginName);
            var code =await CacheHelper.GetAsync<string>(key);

            if (string.IsNullOrEmpty(code) || passcode != code)
                return new Resp(RespTypes.ObjectStateError, "验证码错误");

            await CacheHelper.RemoveAsync(key);
            return new Resp();
        }

        #endregion

    }
}