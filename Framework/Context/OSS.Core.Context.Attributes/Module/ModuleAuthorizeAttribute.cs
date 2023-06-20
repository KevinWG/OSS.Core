using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  应用模块验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class ModuleAuthorizeAttribute : BaseOrderAuthorizeAttribute
    {
        private readonly IModuleAuthProvider _provider;

        /// <inheritdoc />
        public ModuleAuthorizeAttribute(IModuleAuthProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider), "模块的授权实现不能为空!");
            Order     = AttributeConst.Order_Module_AuthAttributeOrder;
        }

        /// <inheritdoc />
        public override  async Task<Resp> Authorize(AuthorizationFilterContext context)
        {
            return await _provider.Authorize();
        }
    }

}
