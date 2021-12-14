using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 接口请求模式验证
    /// </summary>
    public class WebFetchApiAttribute : BaseOrderAuthAttribute
    {

        public WebFetchApiAttribute()
        {
            Order = -99999;
            //p_IsWebSite = true;
        }

        /// <inheritdoc />
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.IsFetchApi())
            {
                var appInfo = context.HttpContext.GetAppIdentity();
                var res     = new Resp(RespTypes.OperateFailed, "当前请求被拒绝!");
                ResponseExceptionEnd(context,appInfo, res);
            }
            return Task.CompletedTask;
        }

    }


}
