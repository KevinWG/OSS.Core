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

using OSS.Common;
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Module.Notify;
using OSS.Core.Module.Notify.Client;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal;

public class AuthService : BaseAuthService, IAuthCommonService
{
    #region 获取登录认证信息

    /// <summary>
    ///  获取授权账号信息
    /// </summary>
    /// <returns></returns>
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        var cacheKey = string.Concat(PortalConst.CacheKeys.Portal_UserIdentity_ByToken, CoreContext.App.Identity.token);
        var getFunc = () =>
        {
            var infoRes = PortalTokenHelper.FormatPortalToken();
            return infoRes.IsSuccess()
                ? GetAuthIdentityById(infoRes.data.userId, infoRes.data.authType)
                : Task.FromResult((IResp<UserIdentity>)new Resp<UserIdentity>().WithResp(infoRes));
        };
        return getFunc.WithAbsoluteCacheAsync(cacheKey, TimeSpan.FromMinutes(5));
    }

    private static async Task<IResp<UserIdentity>> GetAuthIdentityById(long userId, PortalAuthorizeType authType)
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
                identityRes = new Resp<UserIdentity>().WithResp(RespCodes.UserUnLogin, "未知用户信息，请重新登录!");
                break;
        }

        if (!identityRes.IsSuccess() && !identityRes.IsRespCode(RespCodes.UserBlocked) &&
            !identityRes.IsRespCode(RespCodes.UserUnActive))
        {
            // 正常请求获取登录信息时异常，统一返回未登录错误码
            identityRes.WithResp(RespCodes.UserUnLogin, identityRes.msg);
        }

        return identityRes;
    }

    #endregion

    #region 验证是否可以注册

    /// <summary>
    ///     检查账号是否可以注册
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<IResp> CheckIfCanReg(PortalNameReq req)
    {
        return CheckNameIfCanReg(req);
    }

    #endregion

    #region 动态验证码登录注册实现

    /// <summary>
    ///   发送动态码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<IResp> SendCode(PortalNameReq req)
    {
        var code    = NumHelper.RandomNum();
        var targets = new List<string>() { req.name };

        var authSettingRes = await InsContainer<ISettingCommonService>.Instance.GetAuthSetting();
        if (!authSettingRes.IsSuccess())
            return authSettingRes;

        var setting = authSettingRes.data;
        var templateId = req.type == PortalNameType.Mobile
            ? setting.SmsTemplateId.ToInt64()
            : setting.EmailTemplateId.ToInt64();

        var notifyMsg = new NotifySendReq()
        {
            targets    = targets, template_id = templateId,
            body_paras = new Dictionary<string, string> {{"code", code}}
        };

        var res = await NotifyRemoteClient.Notify.Send(notifyMsg);
        if (!res.IsSuccess())
            return res;

        var key = string.Concat(PortalConst.CacheKeys.Portal_Passcode_ByLoginName, req.name);
        await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));

        return res;
    }


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


    private static readonly PortalTokenResp _codeLoginError = new PortalTokenResp(RespCodes.ParaError, "手机号未注册或验证码错误！");

    /// <summary>
    ///   用户动态码登录注册（如果不存在则直接注册
    /// </summary>
    /// <returns></returns>
    public async Task<PortalTokenResp> CodeRegOrLogin(PortalPasscodeReq req)
    {
        var codeRes = await CheckPasscode(req.name, req.code);
        if (!codeRes.IsSuccess())
            return new PortalTokenResp().WithResp(codeRes);

        var userRes = await m_AuthRep.GetUserByLoginType(req.name, req.type);
        if (userRes.IsSuccess())
            return await LoginFinallyExecute(userRes.data, false, req.is_social_bind);

        if (!userRes.IsRespCode(RespCodes.OperateObjectNull))
            return _codeLoginError;

        // 执行注册
        var userInfo = GetRegisterUserInfo(req.name, string.Empty, req.type);
        return await RegFinallyExecute(userInfo, req.is_social_bind);
    }

    //  动态码验证登录
    private async Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req, bool isAdmin)
    {
        var codeRes = await CheckPasscode(req.name, req.code);
        if (!codeRes.IsSuccess())
            return new PortalTokenResp().WithResp(codeRes);

        var userRes = await m_AuthRep.GetUserByLoginType(req.name, req.type);

        return userRes.IsSuccess()
            ? await LoginFinallyExecute(userRes.data, isAdmin, req.is_social_bind)
            : _codeLoginError;
    }

   

    /// <summary>
    ///     验证动态码是否正确
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="passcode"></param>
    /// <returns></returns>
    private static async Task<IResp> CheckPasscode(string loginName, string passcode)
    {
        var key  = string.Concat(PortalConst.CacheKeys.Portal_Passcode_ByLoginName, loginName);
        var code = await CacheHelper.GetAsync<string>(key);

        if (string.IsNullOrEmpty(code) || passcode != code)
            return new Resp(RespCodes.OperateFailed, "验证码错误");

        await CacheHelper.RemoveAsync(key);
        return Resp.DefaultSuccess;
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
        var userInfo = GetRegisterUserInfo(req.name, req.password, req.type);

        userInfo.status = UserStatus.WaitActive; //  默认待激活状态

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
        var userRes = await m_AuthRep.GetUserByLoginType(req.name, req.type);
        if (userRes.IsSuccess())
        {
            var user = userRes.data;
            if (Md5.EncryptHexString(req.password) == user.pass_word)
            {
                return await LoginFinallyExecute(user, isAdmin, req.is_social_bind);
            }
        }

        return new PortalTokenResp(RespCodes.ParaError, "账号密码不正确！");
    }


    #endregion

}