using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IAuthBindingOpenService
{
    /// <summary>
    ///  获取当前登录用户信息
    /// </summary>
    /// <returns></returns>
    Task<Resp<UserBasicMo>> GetCurrentUser();

    /// <summary>
    ///  直接添加用户（管理员权限
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp<long>> AddUser(AddUserReq req);

    /// <summary>
    ///  发送当前登录账号动态码
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<IResp> SendOldCode(PortalNameType type);

    /// <summary>
    ///  发送新账号动态码
    /// </summary>
    /// <param name="portalName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<IResp> SendNewCode(PortalNameType type, string portalName);

    /// <summary>
    ///  获取绑定信息的令牌
    /// </summary>
    /// <param name="oldCode"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<GetBindTokenResp> GetBindToken(string oldCode, PortalNameType type);

    /// <summary>
    ///  绑定信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> BindByCode(BindByPassCodeReq req);
}