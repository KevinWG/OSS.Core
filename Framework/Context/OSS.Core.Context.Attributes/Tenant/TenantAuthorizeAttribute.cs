using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  租户授权过滤器
/// </summary>
public class TenantAuthorizeAttribute : BaseOrderAuthorizeAttribute
{
    private readonly ITenantAuthProvider _provider;

    /// <summary>
    ///  构造函数
    /// </summary>
    public TenantAuthorizeAttribute(ITenantAuthProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider), "租户的授权实现不能为空!");
        Order     = AttributeConst.Order_Tenant_AuthAttribute;
    }

    /// <summary>
    ///  租户的授权判断处理
    /// </summary>
    public override async Task<Resp> Authorize(AuthorizationFilterContext context)
    {
        var tIdentityRes =await _provider.GetIdentity();
        if (!tIdentityRes.IsSuccess())
            return tIdentityRes;

        var tIdentity   = CoreContext.Tenant.Identity = tIdentityRes.data!;
        var appIdentity = CoreContext.App.Identity;

        if (tIdentity.type> appIdentity.ask_meta.tenant_type)
        {
            return new Resp(SysRespCodes.NotAllowed, "抱歉，无访问权限!");
        }
        
        return tIdentityRes;
    }
}