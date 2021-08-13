using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Infrastructure.Web.Attributes
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
        public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="res"></param>
        protected void ResponseExceptionEnd(AuthorizationFilterContext context, Resp res)
        {
            throw new RespException(res.sys_ret, res.ret, res.msg);
        }

    }



}
