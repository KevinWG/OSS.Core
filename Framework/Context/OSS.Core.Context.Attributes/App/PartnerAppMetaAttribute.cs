using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  合作应用信息设置过滤器
    /// </summary>
    public class PartnerAppMetaAttribute : BaseOrderAuthorizeAttribute
    {
        /// <summary>
        /// 外部应用
        /// </summary>
        public PartnerAppMetaAttribute()
        {
            Order = AttributeConst.Order_App_MetaAttribute;
        }

        public override Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            if (!CoreContext.App.IsInitialized)
                CoreContext.App.Identity = new AppIdentity();
            
            var sysInfo = CoreContext.App.Identity;

            sysInfo.auth_mode = AppAuthMode.PartnerContract;
            sysInfo.app_type  = AppType.Single;
            sysInfo.UDID      = "WEB";

            return Task.FromResult(Resp.DefaultSuccess);
        }
    }
}