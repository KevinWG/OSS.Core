using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    public class AppPartnerNameAttribute : BaseOrderAuthAttribute
    {
        private readonly string _appId;
        private readonly string _appVer;

        /// <summary>
        ///  专属回调初始化处理过滤器
        /// </summary>
        /// <param name="fromApp">来源平台名称</param>
        /// <param name="fromAppVer"></param>
        public AppPartnerNameAttribute(string fromApp, string fromAppVer = null)
        {
            _appId  = string.Concat("Partner_", fromApp);
            _appVer = fromAppVer ?? string.Empty;

            p_Order = -1001;
        }


        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sysInfo = AppWebInfoHelper.InitialDefaultAppIdentity(context.HttpContext);

            sysInfo.app_id  = _appId;
            sysInfo.app_ver = _appVer;

            return Task.CompletedTask;
        }
    }
}
