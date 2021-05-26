using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    public class ModuleMetaAttribute: BaseOrderAuthAttribute
    {
        private readonly string _moduleName;
        private readonly AppType _appType;

        public ModuleMetaAttribute(string moduleName)
            :this(moduleName,AppType.Outer)
        {
        }

        public ModuleMetaAttribute(string moduleName, AppType appType)
        {
            p_Order     = -101;
            _appType    = appType;
            _moduleName = moduleName;
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (string.IsNullOrEmpty(_moduleName))
                throw new NullReferenceException("请在当前Controller或父类中使用ModuleNameAttribute标注模块名称！");

            var appInfo = AppWebInfoHelper.GetOrSetAppIdentity(context.HttpContext);
            if (appInfo.app_type > _appType)
            {
                ResponseExceptionEnd(context,new Resp(RespTypes.NoPermission,"当前应用类型，无此接口权限！"));
                return Task.CompletedTask;
            }

            appInfo.module_name = _moduleName;
            return Task.CompletedTask;
        }
    }
}
