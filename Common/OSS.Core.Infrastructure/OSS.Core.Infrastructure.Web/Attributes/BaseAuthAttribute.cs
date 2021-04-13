using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Infrastructure.Web.Attributes
{

    public abstract class BaseOrderAuthAttribute : BaseAuthAttribute, IOrderedFilter
    {
        protected int p_Order=0;
        public int Order => p_Order;
    }

    public abstract class BaseAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        //protected bool p_IsWebSite { get; set; } = false;
        protected void ResponseExceptionEnd(AuthorizationFilterContext context, Resp res)
        {
            if (res.IsRespType(RespTypes.UnLogin)) {
                context.Result = new JsonResult(res);
                return;
            }

            throw new RespException(res.sys_ret,res.ret,res.msg);
            //var result = GetRespResult(context.HttpContext, res);
            //if (result!=null)
            //{
            //    context.Result = result;
            //}
        }

        public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);

    }



}
