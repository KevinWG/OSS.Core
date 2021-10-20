using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Attributes.Helper;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseOrderAuthAttribute : BaseAuthAttribute, IOrderedFilter
    {
        protected int p_Order=0;
        public int Order => p_Order;
    }

    /// <summary>
    ///  基础验证属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class BaseAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 授权方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appInfo"></param>
        /// <param name="res"></param>
        protected void ResponseExceptionEnd(AuthorizationFilterContext context, AppIdentity appInfo, Resp res)
        {
            string rUrl = res.IsRespType(RespTypes.UserUnLogin) 
                ? InterReqHelper.GetNotFoundOrErrorPage(context.HttpContext, appInfo, res) 
                : InterReqHelper.GetNotFoundOrErrorPage(context.HttpContext, appInfo, res);
            
            if (string.IsNullOrEmpty(rUrl))
            {
                context.Result = new ContentResult()
                {
                    ContentType = "application/json; charset=utf-8",
                    Content = $"{{\"ret\":{res.ret},\"msg\":\"{res.msg}\"}}"
                };
                return;
            }

            context.Result = new RedirectResult(rUrl);
            return;
        }




    }



}
