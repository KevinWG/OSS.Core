using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  应用插件的权限验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class ModuleAuthorizeAttribute : BaseOrderAuthorizeAttribute
    {
        private readonly ModuleAuthOption _option;

        /// <inheritdoc />
        public ModuleAuthorizeAttribute(ModuleAuthOption option)
        {
            Order   = AttributeConst.Order_Module_AuthAttributeOrder;
            _option = option;
        }

        /// <inheritdoc />
        public override  Task<Resp> Authorize(AuthorizationFilterContext context)
        {
            var appInfo = CoreContext.App.Identity;
            if (appInfo.app_type == AppType.SystemPlatform || _option.Provider==null)
                return Task.FromResult(Resp.Success());

            return _option.Provider.Authorize();
        }
    }

    /// <summary>
    /// 模块验证的选项信息
    /// </summary>
    public class ModuleAuthOption 
    {
        /// <summary>
        ///  模块验证的实现提供者
        /// </summary>
        public IModuleAuthProvider? Provider { get; set; }
    }
}
