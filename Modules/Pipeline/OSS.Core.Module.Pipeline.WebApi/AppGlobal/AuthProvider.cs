using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;


namespace OSS.Core.Module.Pipeline;

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
    public Task<IResp<AppAccess>> GetByKey(string key)
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
                return Task.FromResult<IResp<AppAccess>>(new Resp<AppAccess>(access));
            }
        }
        return Task.FromResult<IResp<AppAccess>>(new Resp<AppAccess>().WithResp(SysRespCodes.NotAllowed, "非法的请求"));
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
        // 可自定义如下返回：
        var identity = new UserIdentity()
        {
            id = "1",
            name = "测试管理员",
            auth_type = PortalAuthorizeType.Admin
        };

        IResp<UserIdentity> res = new Resp<UserIdentity>(identity);
        return Task.FromResult(res);

        throw new NotImplementedException("请实现 UserAuthProvider 验证方法");
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
        // 可以自定义实现,如：
        IResp<FuncDataLevel> res = new Resp<FuncDataLevel>(FuncDataLevel.All);
        return Task.FromResult(res);

        throw new NotImplementedException("请实现 FuncAuthProvider 验证方法");
    }
}


