using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    public class AppAuthAttribute: BaseOrderAuthAttribute
    {
        private readonly AppAuthOption _appOption;



        public AppAuthAttribute(AppAuthOption appOption)
        {
            if (appOption.AppProvider == null && !appOption.IsWebSite)
                throw new Exception("服务接口 AppAuthOption 中 AppProvider 接口对象必须提供！");
            
            p_Order = -1000;
            _appOption = appOption;
            p_IsWebSite = appOption.IsWebSite;
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 0. app 初始化
            var appInfo = AppWebInfoHelper.InitialDefaultAppIdentity(context.HttpContext);
         
            // 1. app验证
            var res = await AppAuthHelper.FormatAndCheck(context.HttpContext, appInfo, _appOption);
            if (!res.IsSuccess())
            {
                ResponseEnd(context,res);
                return;
            }

            //2. Tenant 验证
            res = await TenantAuthHelper.FormatAndCheck(context.HttpContext, appInfo, _appOption);
            if (!res.IsSuccess())
            {
                ResponseEnd(context, res);
            }
        }

       
    }


    public class AppAuthOption: BaseAuthOption
    {

        public IAppAuthProvider AppProvider { get; set; }

        public ITenantAuthProvider TenantProvider { get; set; }
    }

    public abstract class BaseAuthOption
    {
        /// <summary>
        ///  如果 IsWebSite=True 且当前请求不为ajax时，验证失败时返回json格式，否则直接重定向到 login、404、error 页面
        /// 且 如果IsWebSite=True 无头部ticket，会通过web配置补充appid等信息
        /// </summary>
        public bool IsWebSite { get; set; }
    }


    public class AppPartnerNameAttribute : BaseOrderAuthAttribute
    {
        private readonly string _appId;
        private readonly string _appVer;

        /// <summary>
        ///  专属回调初始化处理过滤器
        /// </summary>
        /// <param name="fromApp">来源平台名称</param>
        /// <param name="fromAppVer"></param>
        public AppPartnerNameAttribute(string fromApp, string fromAppVer = null)
        {
            _appId = string.Concat("Partner_", fromApp);
            _appVer = fromAppVer ?? string.Empty;

            p_Order = -1001;
        }


        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sysInfo = AppWebInfoHelper.InitialDefaultAppIdentity(context.HttpContext);

            sysInfo.app_id = _appId;
            sysInfo.app_ver = _appVer;

            return Task.CompletedTask;
        }
    }
}
