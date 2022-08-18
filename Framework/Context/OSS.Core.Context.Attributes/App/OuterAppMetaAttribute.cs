using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  合作应用信息设置过滤器
    /// </summary>
    public class OuterAppMetaAttribute : BaseOrderAuthorizeAttribute
    {
        /// <summary>
        /// 外部应用
        /// </summary>
        public OuterAppMetaAttribute()
        {
            Order = AttributeConst.Order_App_MetaAttribute;
        }

        public override Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            if (!CoreContext.App.IsInitialized)
                CoreContext.App.Identity = new AppIdentity();
            
            var sysInfo = CoreContext.App.Identity;

            sysInfo.auth_mode = AppAuthMode.OutApp;
            sysInfo.app_type  = AppType.Single;
            sysInfo.UDID      = "WEB";

            return Task.FromResult(Resp.DefaultSuccess);
        }
    }

    /// <summary>
    ///   应用初始配置属性
    /// </summary>
    public class AppMetaAttribute : BaseOrderAuthorizeAttribute
    {
        public AppMetaAttribute(AppType type) : this(AppAuthMode.Browser, type)
        {
        }
        public AppMetaAttribute(AppAuthMode authMode) : this(authMode, AppType.Single)
        {
        }
        public AppMetaAttribute(AppAuthMode authMode, AppType type)
        {
            Order = AttributeConst.Order_App_MetaAttribute;

            _authMode = authMode;
            _type     = type;
        }

        private readonly AppAuthMode _authMode;
        private readonly AppType     _type;

        /// <inheritdoc />
        public override Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            if (!CoreContext.App.IsInitialized)
                CoreContext.App.Identity = new AppIdentity();

            var sysInfo = CoreContext.App.Identity;

            sysInfo.ask_auth.app_type  = _type;
            sysInfo.ask_auth.app_auth_mode = _authMode;

            return Task.FromResult(Resp.DefaultSuccess);
        }
    }
}