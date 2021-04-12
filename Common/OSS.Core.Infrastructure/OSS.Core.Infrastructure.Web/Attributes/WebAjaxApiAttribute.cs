using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Extensions;

namespace OSS.Core.Infrastructure.Web.Attributes
{
    public class WebApiAjaxAttribute : BaseOrderAuthAttribute
    {
        public WebApiAjaxAttribute()
        {
            p_Order = -99999;
            //p_IsWebSite = true;
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            if (!context.HttpContext.Request.IsAjaxApi())
            {
                var res = new Resp(RespTypes.UnKnowOperate, "未知操作!");
                ResponseExceptionEnd(context, res);
            }
            return Task.CompletedTask;
        }

    }


}
