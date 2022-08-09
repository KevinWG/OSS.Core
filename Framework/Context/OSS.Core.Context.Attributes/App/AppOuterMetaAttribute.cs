using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  合作应用信息设置过滤器
    /// </summary>
    public class AppOuterMetaAttribute : BaseOrderAuthorizeAttribute
    {
        /// <summary>
        /// 外部应用
        /// </summary>
        public AppOuterMetaAttribute()
        {
            //_appIdPrefix    = appIdPrefix;
            //_appIdQueryPara = appIdQueryPara;

            Order = AttributeConst.Order_App_OuterMetaAttribute;
        }

        public override Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            var sysInfo = CoreContext.App.Identity;

            sysInfo.source_mode = AppSourceMode.OutApp;
            sysInfo.app_type    = AppType.Single;
            sysInfo.UDID        = "WEB";

            return Task.FromResult(Resp.DefaultSuccess);
        }


    }
}