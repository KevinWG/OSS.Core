using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  功能权限名称过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserFuncMetaAttribute : BaseOrderAuthAttribute
    {
        private readonly string _funcCode;

        /// <summary>
        /// 业务请求中场景值对应的参数
        ///  设置后将从请求参数中获取对应的参数值，记录在上下文的 scene_code 中，方便权限验证处理
        /// </summary>
        public string SceneQueryPara { get; set; }

        /// <summary>
        ///   要求的授权类型
        ///         默认为管理员类型
        /// </summary>
        public PortalAuthorizeType AuthType { get; set; }

        /// <summary>
        /// 功能权限验证
        /// </summary>
        /// <param name="funcCode"></param>
        public UserFuncMetaAttribute(string funcCode)
            : this(PortalAuthorizeType.Admin, funcCode)
        {
        }

        /// <summary>
        /// 功能权限验证
        /// 默认管理员权限
        /// </summary>
        public UserFuncMetaAttribute()
            : this(PortalAuthorizeType.Admin, string.Empty)
        {
        }

        /// <summary>
        /// 功能权限验证
        /// </summary>
        /// <param name="authType">默认管理员权限</param>
        /// <param name="funcCode"></param>
        public UserFuncMetaAttribute(PortalAuthorizeType authType, string funcCode)
        {
            p_Order   = -11;
            _funcCode = funcCode;
            AuthType  = authType;
        }

        /// <inheritdoc />
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sceneCode = string.Empty;
            var appInfo   = context.HttpContext.InitialContextAppIdentity();

            if (!string.IsNullOrEmpty(SceneQueryPara))
            {
                sceneCode = context.HttpContext.Request.Query[SceneQueryPara].ToString();
                if (string.IsNullOrEmpty(sceneCode))
                {
                    ResponseExceptionEnd(context, appInfo, new Resp(RespTypes.ParaError, $"请求要求{SceneQueryPara}对应的参数！"));
                    return Task.CompletedTask;
                }
            }

            appInfo.ask_func = new AskUserFunc(AuthType, _funcCode, sceneCode);
            return Task.CompletedTask;
        }

    }
}
