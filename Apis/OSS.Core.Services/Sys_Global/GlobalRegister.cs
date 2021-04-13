using System;
using System.Net;
using OSS.Common.BasicImpls;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.Services.Basic.Portal.IProxies;
using OSS.Core.Services.Plugs.Config;
using OSS.Core.Services.Plugs.Config.IProxies;
using OSS.Core.Services.Plugs.Log;
using OSS.Core.Services.Plugs.Log.IProxies;
using OSS.Core.Services.Plugs.Notify;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Tools.DirConfig;
using OSS.Tools.Log;

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
            RegisterOSSTools();

            RegisterServiceProxies();
        }

        /// <summary>
        ///  注册服务代理
        /// </summary>
        private static void RegisterServiceProxies()
        {
            //basic-portal
            InsContainer<IPortalServiceProxy>.Set<PortalService>();
            InsContainer<IUserServiceProxy>.Set<UserService>();

            //plugs
            InsContainer<INotifyServiceProxy>.Set<NotifyService>();
            InsContainer<IDirConfigServiceProxy>.Set<DirConfigService>();
            InsContainer<ILogServiceProxy>.Set<LogService>();
        }

        private static void RegisterOSSTools()
        {
            LogHelper.LogWriterProvider = name => SingleInstance<EmailLogProvider>.Instance;
            DirConfigHelper.DirConfigProvider = category => SingleInstance<DirConfigProvider>.Instance;

            LogHelper.LogFormat = log =>
            {
                // 以防全局初始化错误
                var appIdentity = AppReqContext.Identity;
                if (appIdentity == null)
                    return;

                if (!string.IsNullOrEmpty(appIdentity.module_name) && log.source_name == CoreModuleNames.Default)
                {
                    log.source_name = AppReqContext.Identity?.module_name;
                }

                log.log_id = appIdentity.trace_num ?? Guid.NewGuid().ToString();
            };
        }

        /// <summary>
        ///  设置系统相关全局配置
        /// </summary>
        private static void SettingGlobal()
        {
            // 修改底层Http连接数限制
            ServicePointManager.DefaultConnectionLimit = 512;
        }

    }
}
