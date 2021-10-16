using System;
using System.Text;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Tools.Config;

namespace OSS.Core.Infrastructure
{
    /// <summary>
    ///   应用信息辅助类
    /// </summary>
    public static class AppInfoHelper
    {
        /// <summary>
        ///  默认租户id
        /// </summary>
        public const string DefaultTenantId = "1001";

        /// <summary>
        ///  环境变量
        /// </summary>
        public static string EnvironmentName { get; set; } 
        /// <summary>
        /// 是否是开发环境
        /// </summary>
        public static bool IsDev => EnvironmentName == "Development";
        /// <summary>
        /// 是否是生产环境
        /// </summary>
        public static bool IsProduct => EnvironmentName == "Product";

        /// <summary>
        ///  请求接口对应AppId
        /// </summary>
        public static string AppId { get; } = ConfigHelper.GetSection("AppConfig:AppId")?.Value;

        /// <summary>
        ///  请求接口对应AppId的加密秘钥
        /// </summary>
        public static string AppSecret { get; } = ConfigHelper.GetSection("AppConfig:AppSecret")?.Value;

        /// <summary>
        ///  应用工作实例Id
        /// </summary>
        public static int AppWorkerId { get; } = ConfigHelper.GetSection("AppConfig:AppWorkerId")?.Value.ToInt32() ?? 0;

        /// <summary>
        /// 应用版本
        /// </summary>
        public static string AppVersion { get; } = ConfigHelper.GetSection("AppConfig:AppVersion")?.Value ??string.Empty;


        /// <summary>
        ///  格式化应用信息中的
        /// </summary>
        /// <returns></returns>
        public static bool FormatAppIdInfo(AppIdentity app)
        {
            var strArr = app.app_id.Split('0');
            if (strArr.Length != 3)
                return false;

            app.app_type = (AppType) strArr[0].Substring(4).ToCodeNum(_arrCodeStr);

            if (app.app_type != AppType.Proxy)
                app.tenant_id = strArr[1].ToCodeNum(_arrCodeStr).ToString();
            

            //if (!string.IsNullOrEmpty(strArr[2]))
            //    app.client_type = (AppClientType) strArr[2].ToCodeNum(_arrCodeStr);

            return true;
        }

        private const string _arrCodeStr = "5puv6efabcdl12wrsn7xqjkgh3m89tyz";

        /// <summary>
        ///  生成AppId信息
        /// </summary>
        /// <param name="type">应用类型</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static string GenerateAppId(string tenantId, AppType type)
        {
            var appId = new StringBuilder("app_");
            var timespan = DateTime.Now.ToUtcMilliSeconds();

            if (type == AppType.Proxy)
                tenantId = string.Empty;

            appId.Append(((long) type).ToCode(_arrCodeStr)).Append("0");
            appId.Append(tenantId.ToInt64().ToCode(_arrCodeStr)).Append("0");
            appId.Append(timespan.ToCode(_arrCodeStr));

            return appId.ToString();
        }
    }

}
