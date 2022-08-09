﻿using Microsoft.AspNetCore.Mvc.Filters;
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
        private ModuleAuthOption _option;

        public ModuleAuthorizeAttribute(ModuleAuthOption option)
        {
            Order   = AttributeConst.Order_Module_AuthAttributeOrder;
            _option = option;
        }

        public override  Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            var appInfo = CoreContext.App.Identity;
            if (appInfo.app_type == AppType.SystemManager || _option.ModuleProvider==null)
                return AttributeConst.TaskSuccessResp;

            return _option.ModuleProvider.ModuleAuthorize();
        }
    }

    public class ModuleAuthOption 
    {
        public IModuleAuthProvider? ModuleProvider { get; set; }
    }
}