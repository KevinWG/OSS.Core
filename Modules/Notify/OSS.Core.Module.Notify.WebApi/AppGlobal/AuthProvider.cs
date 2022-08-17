using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Module.Portal.Client;

namespace OSS.Core.Module.Notify;

/// <summary>
///  登录用户授权验证
/// </summary>
public class UserAuthProvider : IUserAuthProvider
{
    /// <inheritdoc />
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        // 引用 Portal 接口客户端SDK
        return PortalRemoteClient.Auth.GetIdentity();
    }
}

/// <summary>
///  登录用户功能授权验证
/// </summary>
public class FuncAuthProvider : IFuncAuthProvider
{
    /// <inheritdoc />
    public Task<IResp<FuncDataLevel>> Authorize(string funcCode, string sceneCode)
    {
        // 引用 Portal 接口客户端SDK
        return PortalRemoteClient.Permit.CheckPermit(funcCode, sceneCode);
    }
}