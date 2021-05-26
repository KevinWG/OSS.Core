using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    /// <summary>
    ///  应用插件的权限验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class ModuleAuthAttribute : BaseOrderAuthAttribute
    {
        private ModuleAuthOption _option;

        public ModuleAuthAttribute(ModuleAuthOption option)
        {
            p_Order = -100;
            _option = option;
            //p_IsWebSite = option.IsWebSite;
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appInfo = CoreAppContext.Identity;
            if (appInfo.app_type == AppType.SystemManager)
                return Task.CompletedTask;
            
            //  todo  判断AppId是否具有操作 此模块的权限
            return Task.CompletedTask;
        }
    }

    public class ModuleAuthOption 
    {
        public IModuleAuthProvider ModuleProvider { get; set; }
    }
}
