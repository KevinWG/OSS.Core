using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  合作应用信息设置过滤器
    /// </summary>
    public class AppOuterMetaAttribute : BaseOrderAuthAttribute
    {
        //private readonly string _appIdPrefix;
        //private readonly string _appIdQueryPara;
        
        ///// <summary>
        /////  合作应用信息设置过滤器
        ///// </summary>
        ///// <param name="fromAppId">来源平台名称</param>
        //public AppOuterMetaAttribute(string fromAppId) : this(fromAppId, string.Empty)
        //{
        //}

        /// <summary>
        /// 外部应用
        /// </summary>
        public AppOuterMetaAttribute()
        {
            //_appIdPrefix    = appIdPrefix;
            //_appIdQueryPara = appIdQueryPara;

            Order = -1001;
        }

        /// <inheritdoc />
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sysInfo = context.HttpContext.GetAppIdentity();

            sysInfo.source_mode = AppSourceMode.OutApp;
            sysInfo.app_type   = AppType.Single;
            sysInfo.UDID       = "WEB";

            return Task.CompletedTask;
        }
    }
}