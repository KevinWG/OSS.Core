using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Extensions;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes
{

    public abstract class BaseOrderAuthAttribute : BaseAuthAttribute, IOrderedFilter
    {
        protected int p_Order=0;
        public int Order => p_Order;
    }

    public abstract class BaseAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        protected bool p_IsWebSite { get; set; } = false;
        protected void ResponseEnd(AuthorizationFilterContext context, Resp res)
        {
            var result = GetRespResult(context.HttpContext, res);
            if (result!=null)
            {
                context.Result = result;
            }
        }

        protected void ResponseExceptionEnd(ExceptionContext context, Resp res)
        {
            var result = GetRespResult(context.HttpContext, res);
            if (result != null)
            {
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }

        private ActionResult GetRespResult(HttpContext context, Resp res)
        {
            // 服务类接口直接返回
            if (!p_IsWebSite)
            {
                return new JsonResult(res);
            }

            var req    = context.Request;
            var isAjax = req.IsApiAjax();

            if (isAjax)
            {
                res.msg = AppWebInfoHelper.GetRedirectUrl(context, res, true);
                return new JsonResult(res);
            }

            if (AppWebInfoHelper.CheckWebUnRedirectUrl(req.Path))
                return null;

            var redirectUrl = AppWebInfoHelper.GetRedirectUrl(context, res, false);
            return new RedirectResult(redirectUrl);
        }


        public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);

    }
}
