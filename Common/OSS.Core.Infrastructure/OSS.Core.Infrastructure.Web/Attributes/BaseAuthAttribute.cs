using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extention;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Helpers;

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
