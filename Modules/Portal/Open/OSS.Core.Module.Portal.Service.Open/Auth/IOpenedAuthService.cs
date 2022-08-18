using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal;

public interface IOpenedAuthService
{
    /// <summary>
    ///  获取授权账号信息
    /// </summary>
    /// <returns></returns>
    Task<IResp<UserIdentity>> GetIdentity();

    /// <summary>
    ///     检查账号是否可以注册
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> CheckIfCanReg(PortalNameReq req);

    /// <summary>
    ///     用户动态码登录
    /// </summary>
    /// <returns></returns>
    Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req);

    /// <summary>
    /// 管理员动态码登录
    /// </summary>
    /// <returns></returns>
    Task<PortalTokenResp> CodeAdminLogin(PortalPasscodeReq req);

    /// <summary>
    ///   用户动态码登录注册（如果不存在则直接注册
    /// </summary>
    /// <returns></returns>
    Task<PortalTokenResp> CodeRegOrLogin(PortalPasscodeReq req);

    /// <summary>
    ///     发送动态码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> SendCode(PortalNameReq req);

    /// <summary>
    ///   直接通过账号密码注册用户信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PortalTokenResp> PwdReg(PortalPasswordReq req);

    /// <summary> 
    ///     用户密码登录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PortalTokenResp> PwdLogin(PortalPasswordReq req);

    /// <summary>
    ///     管理员用户密码登录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PortalTokenResp> PwdAdminLogin(PortalPasswordReq req);
}