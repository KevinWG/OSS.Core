﻿using Microsoft.AspNetCore.Mvc.Filters;
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

        /// <inheritdoc />
        public override Task<Resp> Authorize(AuthorizationFilterContext context)
        {
            if (!CoreContext.App.IsInitialized)
                CoreContext.App.Identity = new AppIdentity();
            
            var sysInfo = CoreContext.App.Identity;

            sysInfo.auth_mode = AppAuthMode.PartnerContract;
            sysInfo.type  = AppType.Normal;
            sysInfo.UDID      = "WEB";

            return Task.FromResult(Resp.Success());
        }
    }
}