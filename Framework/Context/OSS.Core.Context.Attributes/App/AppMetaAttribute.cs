using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///   应用初始配置属性
/// </summary>
public class AppMetaAttribute : BaseOrderAuthorizeAttribute
{
    /// <inheritdoc />
    public AppMetaAttribute(AppType type) : this(AppAuthMode.None, type)
    {
    }

    /// <inheritdoc />
    public AppMetaAttribute(AppAuthMode authMode) : this(authMode, AppType.Normal)
    {
    }

    /// <inheritdoc />
    public AppMetaAttribute(AppAuthMode authMode, AppType type)
    {
        Order = AttributeConst.Order_App_MetaAttribute;

        _authMode = authMode;
        _type     = type;
    }

    private readonly AppAuthMode _authMode;
    private readonly AppType     _type;

    /// <inheritdoc />
    public override Task<Resp> Authorize(AuthorizationFilterContext context)
    {
        if (!CoreContext.App.IsInitialized)
            CoreContext.App.Identity = new AppIdentity();

        var sysInfo = CoreContext.App.Identity;

        sysInfo.ask_auth.app_type      = _type;
        sysInfo.ask_auth.app_auth_mode = _authMode;

        return Task.FromResult(Resp.Success());
    }
}