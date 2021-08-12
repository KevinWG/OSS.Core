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
        private readonly string _queryCode;

        private readonly PortalAuthorizeType _auth_type;
        
        /// <summary>
        ///  功能权限验证
        /// </summary>
        /// <param name="authType"></param>
        public UserFuncAttribute(PortalAuthorizeType authType):this(authType,string.Empty,String.Empty)
        {
        }

        /// <summary>
        /// 功能权限验证
        /// </summary>
        /// <param name="funcCode"></param>
        /// <param name="queryCode"></param>
        public UserFuncAttribute( string funcCode, string queryCode = null):this(PortalAuthorizeType.Admin,funcCode,queryCode)
        {
        }

        /// <summary>
        /// 功能权限验证
        /// </summary>
        /// <param name="authType"></param>
        /// <param name="funcCode"></param>
        /// <param name="queryCode"></param>
        public UserFuncAttribute(PortalAuthorizeType authType, string funcCode, string queryCode = null)
        {
            p_Order = -11;

            _funcCode  = funcCode;
            _queryCode = queryCode;
            _auth_type = authType;
        }

        /// <inheritdoc />
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appIdentity = CoreAppContext.Identity;

            appIdentity.ask_func = new AskUserFunc(_auth_type,_funcCode, _queryCode);

            return Task.CompletedTask;
        }

    }
}
