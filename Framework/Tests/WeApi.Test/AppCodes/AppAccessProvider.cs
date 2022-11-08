using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;

namespace WeApi.Test;


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



public class TenantAuthProvider : ITenantAuthProvider
{
    public async Task<IResp<TenantIdentity>> GetIdentity()
    {
        return new Resp<TenantIdentity>(new TenantIdentity()
        {
            id = "1"
        });
    }
}
