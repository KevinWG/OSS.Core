using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
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
        private readonly string _sceneQueryPara;

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
        /// <param name="sceneQueryPara">业务 QueryCode 的参数 </param>
        public UserFuncAttribute( string funcCode, string sceneQueryPara = null):this(PortalAuthorizeType.Admin,funcCode, sceneQueryPara)
        {
        }

        /// <summary>
        /// 功能权限验证
        /// </summary>
        /// <param name="authType"></param>
        /// <param name="funcCode"></param>
        /// <param name="sceneQueryPara">业务 QueryCode 的参数 </param>
        public UserFuncAttribute(PortalAuthorizeType authType, string funcCode, string sceneQueryPara = null)
        {
            p_Order = -11;

            _funcCode  = funcCode;
            _auth_type = authType;

            _sceneQueryPara = sceneQueryPara;
        }

        /// <inheritdoc />
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sceneCode = string.Empty;
            if (!string.IsNullOrEmpty(_sceneQueryPara))
            {
                sceneCode = context.HttpContext.Request.Query[_sceneQueryPara].ToString();
                if (string.IsNullOrEmpty(sceneCode))
                {
                    ResponseExceptionEnd(context,new Resp(RespTypes.ParaError,$"请求参数中的权限业务码({_sceneQueryPara})不能为空！"));
                    return Task.CompletedTask;
                }
            }

            var appIdentity = CoreAppContext.Identity;
            appIdentity.ask_func = new AskUserFunc(_auth_type,_funcCode, sceneCode);

            return Task.CompletedTask;
        }

    }
}
