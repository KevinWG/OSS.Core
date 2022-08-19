using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Core.Module.Portal.Client;
using OSS.Tools.Config;

namespace OSS.Core.Module.Notify;

/// <summary>
///  应用访问秘钥信息提供者
/// </summary>
public class AppAccessProvider : IAppAccessProvider
{
    private static List<AppAccess>? _appAccessList = null;
    public async Task<IResp<AppAccess>> GetByKey(string key)
    {
        if (_appAccessList == null)
        {
            _appAccessList = new List<AppAccess>();
            ConfigHelper.Configuration.GetSection("Access").Bind(_appAccessList);
        }

        foreach (var access in _appAccessList)
        {
            if (access.access_key == key)
            {
                return new Resp<AppAccess>(access);
            }
        }

        return new Resp<AppAccess>().WithResp(SysRespCodes.NotAllowed, "非法的请求");
    }
}


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