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

    /// <inheritdoc />
    public Task<IResp<AppAccess>> GetByKey(string key)
    {
        if (_appAccessList == null)
        {
            _appAccessList = new List<AppAccess>();
            ConfigHelper.Configuration.GetSection("Access").Bind(_appAccessList);
        }

        var access = _appAccessList.FirstOrDefault(a => a.access_key == key);
        var res = access == null
            ? new Resp<AppAccess>().WithResp(SysRespCodes.NotAllowed, "非法的请求")
            : new Resp<AppAccess>(access);

        return Task.FromResult<IResp<AppAccess>>(res);
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
    public Task<IResp<FuncDataLevel>> Authorize(string funcCode)
    {
        // 引用 Portal 接口客户端SDK
        return PortalRemoteClient.Grant.CheckPermit(funcCode);
    }
}