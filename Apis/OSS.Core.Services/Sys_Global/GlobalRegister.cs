using System;
using System.Net;
using OSS.Common.BasicImpls;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.Services.Basic.Portal.IProxies;
using OSS.Core.Services.Plugs.Notify;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Tools.DirConfig;
using OSS.Tools.Log;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Sys_Global.Log;
using OSS.Core.Services.Basic.Permit.Proxy;
using OSS.Core.Services.Basic.Permit;

namespace OSS.Core.Services.Sys_Global
{
    public class GlobalRegister
    {
        /// <summary>
        /// 注册全局配置等相关实现
        /// </summary>
        public static void RegisterConfig()
        {
            SettingGlobal();
            RegisterServiceProxies();
            RegisterOSSTools();
        }

        /// <summary>
        ///  注册服务代理
        /// </summary>
        private static void RegisterServiceProxies()
        {
            //basic
            InsContainer<IPortalServiceProxy>.Set<PortalService>();
            InsContainer<IUserServiceProxy>.Set<UserService>();

            InsContainer<IPermitService>.Set<PermitService>();

            // core


            //plugs
            InsContainer<INotifyServiceProxy>.Set<NotifyService>();
        }

        private static void RegisterOSSTools()
        {
            LogHelper.LogWriterProvider = name => SingleInstance<EmailLogProvider>.Instance;
            DirConfigHelper.DirConfigProvider = category => SingleInstance<DirConfigProvider>.Instance;

            LogHelper.LogFormat = log =>
            {
                // 以防全局初始化错误
                var appIdentity = CoreAppContext.Identity;
                if (appIdentity == null)
                    return;

                if (!string.IsNullOrEmpty(appIdentity.module_name) && log.source_name == CoreModuleNames.Default)
                {
                    log.source_name = CoreAppContext.Identity?.module_name;
                }

                log.log_id = appIdentity.trace_num ?? Guid.NewGuid().ToString();
            };
        }

        /// <summary>
        ///  设置系统相关全局配置
        /// </summary>
        private static void SettingGlobal()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // 修改底层Http连接数限制
            ServicePointManager.DefaultConnectionLimit = 512;
        }

    }
}
