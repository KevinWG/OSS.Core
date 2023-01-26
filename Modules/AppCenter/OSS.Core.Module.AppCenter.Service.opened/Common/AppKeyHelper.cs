
using OSS.Common.Extension;
using OSS.Core.Context;
using System.Text;

namespace OSS.Core.Module.AppCenter
{
    public static class AppKeyHelper
    {
        /// <summary>
        ///  格式化应用信息中的
        /// </summary>
        /// <returns></returns>
        public static bool FormatAppKeyInfo(AppIdentity app)
        {
            var strArr = app.access_key.Split('0');
            if (strArr.Length != 3)
                return false;

            app.app_type = (AppType)strArr[0].Substring(4).ToCodeNum(_arrCodeStr);

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
            var appKey    = new StringBuilder("app_");
            var timespan = DateTime.Now.ToUtcMilliSeconds();

            if (type == AppType.Proxy)
                tenantId = string.Empty;

            appKey.Append(((long)type).ToCode(_arrCodeStr)).Append("0");
            appKey.Append(tenantId.ToInt64().ToCode(_arrCodeStr)).Append("0");
            appKey.Append(timespan.ToCode(_arrCodeStr));

            return appKey.ToString();
        }

    }
}
