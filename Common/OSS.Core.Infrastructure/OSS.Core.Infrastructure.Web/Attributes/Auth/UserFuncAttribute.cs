using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Core.Context;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    /// <summary>
    ///  功能权限名称过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserFuncAttribute : BaseOrderAuthAttribute
    {
        private readonly string _funcCode;

        public UserFuncAttribute(string funcCode)
        {
            p_Order = -11;
            _funcCode = funcCode;
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appIdentity = CoreAppContext.Identity;
            if (string.IsNullOrEmpty(appIdentity.ask_func))
            { 
                // 非需授权认证请求
                appIdentity.ask_func = _funcCode;
            }
            return Task.CompletedTask;
        }

    }
}
