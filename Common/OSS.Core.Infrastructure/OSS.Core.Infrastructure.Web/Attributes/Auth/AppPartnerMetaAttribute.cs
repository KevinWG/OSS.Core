using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    /// <summary>
    ///  合作应用信息设置过滤器
    /// </summary>
    public class AppOuterMetaAttribute : BaseOrderAuthAttribute
    {
        private readonly string _appIdPrefix;
        private readonly string _appIdQueryPara;
        
        /// <summary>
        ///  合作应用信息设置过滤器
        /// </summary>
        /// <param name="fromAppId">来源平台名称</param>
        public AppOuterMetaAttribute(string fromAppId) : this(fromAppId, string.Empty)
        {
        }

        /// <summary>
        /// 合作应用信息设置过滤器
        /// </summary>
        /// <param name="appIdPrefix"></param>
        /// <param name="appIdQueryPara"></param>
        public AppOuterMetaAttribute(string appIdPrefix, string appIdQueryPara)
        {
            _appIdPrefix    = appIdPrefix;
            _appIdQueryPara = appIdQueryPara;

            p_Order = -1001;
        }

        /// <inheritdoc />
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sysInfo = AppWebInfoHelper.GetOrSetAppIdentity(context.HttpContext);

            sysInfo.app_id =
                string.Concat(_appIdPrefix, context.HttpContext.Request.Query[_appIdQueryPara].ToString()); // _appId;

            sysInfo.source_mode = AppSourceMode.OutApp;
            //sysInfo.client_type = AppClientType.Server;
            sysInfo.app_type   = AppType.Single;
            sysInfo.UDID       = "WEB";

            return Task.CompletedTask;
        }
    }
}