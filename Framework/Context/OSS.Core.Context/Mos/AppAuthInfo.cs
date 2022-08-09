using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using System.Text;

namespace OSS.Core.Context
{
    /// <summary>
    /// 应用信息
    /// </summary>
    public class AppAuthInfo
    {
        #region  参与签名属性
      
        /// <summary>
        ///   【请求方】应用Id
        /// </summary>
        public string access_key { get; set; } = string.Empty;

        /// <summary>
        ///  【请求方】应用版本 (可选)
        /// </summary>
        public string app_ver { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳  (必填)
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        ///  用户Token  (需登录接口必填)
        /// </summary>
        public string? token { get; set; } 

        /// <summary>
        /// IP地址 (可选)
        /// </summary>
        public string? client_ip { get; set; }

        /// <summary>
        ///  请求跟踪编号 (唯一值必填)
        /// </summary>
        public string? trace_no { get; set; }

        /// <summary>
        /// 唯一设备Id (可选)
        /// </summary>
        public string? UDID { get; set; }

        /// <summary>
        ///  租户ID  (内部使用，外部无效)
        /// </summary>
        public string? tenant_id { get; set; }

        #endregion
        
        /// <summary>
        ///  sign标识
        /// </summary>
        public string sign { get; set; } = string.Empty;

        /// <summary>
        ///   应用类型 [非外部传值，不参与签名]
        /// </summary>
        public AppType app_type { get; set; } = AppType.Single;
    }


    /// <summary>
    /// 授权信息扩展管理
    /// </summary>
    public static class AppAuthInfoExtension
    {
        #region  字符串处理

        /// <summary>
        ///   从头字符串中初始化签名相关属性信息
        /// </summary>
        /// <param name="appAuthInfo"></param>
        /// <param name="ticket"></param>
        /// <param name="separator">A=a  B=b 之间分隔符</param>
        public static void FromTicket(this AppAuthInfo appAuthInfo, string ticket, char separator = ';')
        {
            if (string.IsNullOrEmpty(ticket)) return;

            var strs = ticket.Split(separator);
            foreach (var str in strs)
            {
                if (string.IsNullOrEmpty(str)) continue;

                var keyValue = str.Split(new[] { '=' }, 2);
                if (keyValue.Length <= 1) continue;

                var val = keyValue[1].SafeUnescapeDataString();

                FormatProperty(appAuthInfo,keyValue[0], val);
            }
        }

        /// <summary>
        ///   格式化属性值
        /// </summary>
        /// <param name="appAuthInfo"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        private static void FormatProperty(AppAuthInfo appAuthInfo,string key, string val)
        {
            switch (key)
            {
                case "access_key":
                    appAuthInfo.access_key = val;
                    break;


                case "app_ver":
                    appAuthInfo.app_ver = val;
                    break;

                case "client_ip":
                    appAuthInfo.client_ip = val;
                    break;

                case "tenant_id":
                    appAuthInfo.tenant_id = val;
                    break;

                case "token":
                    appAuthInfo.token = val;
                    break;
                case "trace_no":
                    appAuthInfo.trace_no = val;
                    break;
                case "timestamp":
                    appAuthInfo.timestamp = val.ToInt64();
                    break;

                case "udid":
                    appAuthInfo.UDID = val;
                    break;
                case "sign":
                    appAuthInfo.sign = val;
                    break;
            }
        }


        #endregion

        #region  签名相关

        private static readonly IResp _successResp = new Resp();


        /// <summary>
        ///   检验是否合法
        /// </summary>
        /// <param name="appAuthInfo"></param>
        /// <param name="accessSecret"></param>
        /// <param name="signExpiredSeconds"></param>
        /// <param name="extSignData">参与签名的扩展数据（ 原签名数据 + "&amp;" + extSignData ）</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static IResp CheckSign(this AppAuthInfo appAuthInfo,string accessSecret, int signExpiredSeconds, string extSignData = null, char separator = ';')
        {
            if (appAuthInfo.timestamp <= 0 || string.IsNullOrEmpty(appAuthInfo.access_key) || string.IsNullOrEmpty(appAuthInfo.trace_no))
                return new Resp(RespCodes.ParaError, "参数错误！");

            if (Math.Abs(DateTime.Now.ToUtcSeconds() - appAuthInfo.timestamp) > signExpiredSeconds)
                return new Resp(RespCodes.ParaExpired, "签名不在时效范围(请使用Unix Timestamp)！");

            var signData = ComputeSign(appAuthInfo,appAuthInfo.access_key, accessSecret, appAuthInfo.app_ver,  extSignData, separator);

            return appAuthInfo.sign == signData ? _successResp : new Resp(RespCodes.ParaSignError, "签名错误！");
        }

        /// <summary>
        /// 生成签名后的字符串
        /// </summary>
        /// <param name="appAuthInfo"></param>
        /// <param name="accessKey">当前应用来源</param>
        /// <param name="appVersion"></param>
        /// <param name="accessSecret"></param>
        /// <param name="extSignData">参与签名的扩展数据（ 原签名数据 + "&amp;" + extSignData ）</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToTicket(this AppAuthInfo appAuthInfo, string accessKey, string accessSecret, string appVersion, string? extSignData = null, char separator = ';')
        {
            appAuthInfo.timestamp = DateTime.Now.ToUtcSeconds();

            appAuthInfo.sign = ComputeSign(appAuthInfo,accessKey, accessSecret, appVersion,  extSignData, separator);

            var ticketStr = GetContent(appAuthInfo,accessKey, appVersion, separator, false);

            AddTicketProperty("sign", appAuthInfo.sign, separator, ticketStr, false);

            return ticketStr.ToString();
        }

        private static string ComputeSign(AppAuthInfo appAuthInfo, string accessKey, string accessSecret, string appVersion,  string? extSignData, char separator)
        {
            var signContent = GetContent(appAuthInfo,accessKey, appVersion, separator, true);
            if (!string.IsNullOrEmpty(extSignData))
                signContent.Append("&").Append(extSignData);

            return HMACSHA.EncryptBase64(signContent.ToString(), accessSecret);
        }


        /// <summary>
        ///   获取要加密签名的串
        /// </summary>
        /// <param name="appAuthInfo"></param>
        /// <param name="accessKey"></param>
        /// <param name="appVersion"></param>
        /// <param name="separator"></param>
        /// <param name="isForSign">是否签名校验时调用，签名校验时不进行url转义，否则需要转义</param>
        /// <returns></returns>
        private static StringBuilder GetContent(AppAuthInfo appAuthInfo, string accessKey, string appVersion, char separator, bool isForSign)
        {
            var strTicketParas = new StringBuilder();

            AddTicketProperty("access_key", accessKey, separator, strTicketParas, isForSign);
            AddTicketProperty("app_ver", appVersion, separator, strTicketParas, isForSign);
            AddTicketProperty("client_ip", appAuthInfo.client_ip, separator, strTicketParas, isForSign);

            if (appAuthInfo.app_type == AppType.Proxy)
            {
                AddTicketProperty("tenant_id", appAuthInfo.tenant_id, separator, strTicketParas, isForSign);
            }

            AddTicketProperty("token", appAuthInfo.token, separator, strTicketParas, isForSign);

            AddTicketProperty("trace_no", appAuthInfo.trace_no, separator, strTicketParas, isForSign);
            AddTicketProperty("timestamp", appAuthInfo.timestamp.ToString(), separator, strTicketParas, isForSign);

            AddTicketProperty("udid", appAuthInfo.UDID, separator, strTicketParas, isForSign);
            return strTicketParas;
        }



        /// <summary>
        ///   追加要加密的串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <param name="strTicketParas"></param>
        /// <param name="isForSign">是否参与加密字符串</param>
        private static void AddTicketProperty(string name, string? value, char separator, StringBuilder strTicketParas,
            bool isForSign)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (strTicketParas.Length > 0)
                strTicketParas.Append(separator);

            strTicketParas.Append(name).Append("=").Append(isForSign ? value : value.SafeEscapeUriDataString());
        }

        #endregion

    }

}
