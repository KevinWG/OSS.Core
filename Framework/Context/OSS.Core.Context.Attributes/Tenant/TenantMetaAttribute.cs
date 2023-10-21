using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  租户预定义配置信息属性
/// </summary>
public class TenantMetaAttribute : BaseOrderAuthorizeAttribute
{
    private readonly TenantType _tenantType;


    /// <summary>
    ///  租户预定义配置信息构造函数
    /// </summary>
    public TenantMetaAttribute():this(TenantType.Normal)
    {
    }

    /// <summary>
    ///  租户预定义配置信息构造函数
    /// </summary>
    public TenantMetaAttribute(TenantType tenantType)
    {
        Order       = AttributeConst.Order_Tenant_MetaAttribute;
        _tenantType = tenantType;
    }

    /// <summary>
    ///  设置租户预定义信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Task<Resp> Authorize(AuthorizationFilterContext context)
    {
        if (!CoreContext.App.IsInitialized)
        {
            CoreContext.App.Identity = new AppIdentity();
        }

        CoreContext.App.Identity.ask_meta.tenant_type = _tenantType;

        // 不要使用否则上下文缺失， Task.FromResult(Resp.Success());
        return Task.FromResult(Resp.Success());
    }
}