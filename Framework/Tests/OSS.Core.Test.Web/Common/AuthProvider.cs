using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;

namespace TM.Modules.Files;

/// <summary>
///  应用访问秘钥信息提供者
/// </summary>
public class AppAccessProvider : IAppAccessProvider
{
    private static List<AppAccess>? _appAccessList = null;

    /// <summary>
    ///  根据客户端请求access_key值获取访问秘钥信息(用于模块间的签名验证
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<Resp<AppAccess>> GetByKey(string key)
    {
        if (_appAccessList == null)
        {
            _appAccessList = new List<AppAccess>();
            ConfigHelper.Configuration?.GetSection("Access").Bind(_appAccessList);
        }

        foreach (var access in _appAccessList.Where(access => access.access_key == key))
        {
            return Task.FromResult(new Resp<AppAccess>(access));
        }

        return Task.FromResult(new Resp<AppAccess>().WithResp(SysRespCodes.NotAllowed, "非法的请求"));
    }
}



public class TenantAuthProvider:ITenantAuthProvider
{
    public Task<Resp<TenantIdentity>> GetIdentity()
    {
        return Task.FromResult(new Resp<TenantIdentity>(new TenantIdentity()
        {
            id   = "1",
            type = TenantType.Normal
        }));
    }
}


/// <summary>
///  登录用户授权验证
/// </summary>
public class UserAuthProvider : IUserAuthProvider
{
    /// <inheritdoc />
    public Task<Resp<UserIdentity>> GetIdentity()
    {
        return Task.FromResult(new Resp<UserIdentity>(new UserIdentity()
        {
            id   = "1",
        }));
    }
}

/// <summary>
///  登录用户功能授权验证
/// </summary>
public class FuncAuthProvider : IFuncAuthProvider
{
    /// <inheritdoc />
    public Task<Resp<FuncDataLevel>> Authorize(string funcCode)
    {
        // 可以自定义实现,如：
        //Resp<FuncDataLevel> res = new Resp<FuncDataLevel>(FuncDataLevel.All);
        //return Task.FromResult(res);

        // 或者部署通用模块 Portal，引用接口客户端SDK，如：
        // return PortalRemoteClient.Grant.CheckPermit(funcCode);

        return Task.FromResult(new Resp<FuncDataLevel>(FuncDataLevel.All));
    }
}


