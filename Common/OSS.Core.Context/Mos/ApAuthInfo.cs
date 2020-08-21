using System;
using System.Text;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Encrypt;
using OSS.Common.Extention;

namespace OSS.Core.Context.Mos
{
    public class AppAuthInfo
    {
        #region  参与签名属性
      
        /// <summary>
        ///   【请求方】应用来源
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        ///  【请求方】应用版本 (可选)
        /// </summary>
        public string app_ver { get; set; }

        /// <summary>
        /// 时间戳  (必填)
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        ///  用户Token  (需登录接口必填)
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// IP地址 (可选)
        /// </summary>
        public string client_ip { get; set; }

        /// <summary>
        ///  请求跟踪编号 (唯一值必填)
        /// </summary>
        public string trace_num { get; set; }

        /// <summary>
        /// 唯一设备Id (可选)
        /// </summary>
        public string UDID { get; set; }

        /// <summary>
        ///  租户ID  (内部使用，外部无效)
        /// </summary>
        public string tenant_id { get; set; }

        #endregion
        
        /// <summary>
        ///  sign标识
        /// </summary>
        public string sign { get; set; }
        
        #region  字符串处理

        /// <summary>
        ///   从头字符串中初始化签名相关属性信息
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="separator">A=a  B=b 之间分隔符</param>
        public void FromTicket(string ticket, char separator = ';')
        {
            if (string.IsNullOrEmpty(ticket)) return;

            var strs = ticket.Split(separator);
            foreach (var str in strs)
            {
                if (string.IsNullOrEmpty(str)) continue;

                var keyValue = str.Split(new[] {'='}, 2);
                if (keyValue.Length <= 1) continue;

                var val = keyValue[1].UrlDecode();
                FormatProperty(keyValue[0], val);
            }
        }

        /// <summary>
        ///   格式化属性值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void FormatProperty(string key, string val)
        {
            switch (key)
            {
                case "appid":
                    app_id = val;
                    break;
                case "appver":
                    app_ver = val;
                    break;
                //case "func":
                //    func = val;
                //    break;
                case "ip":
                    client_ip = val;
                    break;

                case "tenantid":
                    tenant_id = val;
                    break;

                case "token":
                    token = val;
                    break;
                case "tracenum":
                    trace_num = val;
                    break;
                case "timestamp":
                    timestamp = val.ToInt64();
                    break;

                case "udid":
                    UDID = val;
                    break;
                case "sign":
                    sign = val;
                    break;
            }
        }


        #endregion

        #region  签名相关


        /// <summary>
        ///   检验是否合法
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="signExpiredSeconds"></param>
        /// <param name="extSignData">参与签名的扩展数据（ 原签名数据 + "&amp;" + extSignData ）</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public Resp CheckSign(string secretKey, int signExpiredSeconds, string extSignData = null, char separator = ';')
        {
            if (timestamp <= 0 || string.IsNullOrEmpty(app_id) || string.IsNullOrEmpty(trace_num))
                return new Resp(RespTypes.SignExpired, "签名数据不正确！");

            if (Math.Abs(DateTime.Now.ToUtcSeconds() - timestamp) > signExpiredSeconds)
                return new Resp(RespTypes.SignExpired, "签名不在时效范围(请使用Unix Timestamp)！");

            var signData = CompulteSign(app_id, app_ver, secretKey, extSignData, separator);

            return sign == signData ? new Resp() : new Resp(RespTypes.SignError, "签名错误！");
        }

        /// <summary>
        /// 生成签名后的字符串
        /// </summary>
        /// <param name="appId">当前应用来源</param>
        /// <param name="appVersion"></param>
        /// <param name="secretKey"></param>
        /// <param name="extSignData">参与签名的扩展数据（ 原签名数据 + "&amp;" + extSignData ）</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ToTicket(string appId, string appVersion, string secretKey, string extSignData = null,
            char separator = ';')
        {
            timestamp = DateTime.Now.ToUtcSeconds();

            sign = CompulteSign(appId, appVersion, secretKey, extSignData, separator);

            var ticket = GetContent(appId, appVersion, separator, false);
            AddTicketProperty("sign", sign, separator, ticket, false);

            return ticket.ToString();
        }

        private string CompulteSign(string appId, string appVersion, string secretKey, string extSignData, char separator)
        {
            var signContent = GetContent(appId, appVersion, separator, true);
            if (!string.IsNullOrEmpty(extSignData))
                signContent.Append("&").Append(extSignData);

            return HMACSHA.EncryptBase64(signContent.ToString(), secretKey);
        }


        /// <summary>
        ///   获取要加密签名的串
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appVersion"></param>
        /// <param name="separator"></param>
        /// <param name="isForSign">是否签名校验时调用，签名校验时不进行url转义，否则需要转义</param>
        /// <returns></returns>
        private StringBuilder GetContent(string appId, string appVersion, char separator, bool isForSign)
        {
            var strTicketParas = new StringBuilder();

            AddTicketProperty("appid", appId, separator, strTicketParas, isForSign);
            AddTicketProperty("appver", appVersion, separator, strTicketParas, isForSign);
            //AddTicketProperty("func", func, separator, strTicketParas, isForSign);
            AddTicketProperty("ip", client_ip, separator, strTicketParas, isForSign);

            AddTicketProperty("tenantid", tenant_id, separator, strTicketParas, isForSign);
            AddTicketProperty("token", token, separator, strTicketParas, isForSign);

            AddTicketProperty("tracenum", trace_num, separator, strTicketParas, isForSign);
            AddTicketProperty("timestamp", timestamp.ToString(), separator, strTicketParas, isForSign);

            AddTicketProperty("udid", UDID, separator, strTicketParas, isForSign);
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
        private static void AddTicketProperty(string name, string value, char separator, StringBuilder strTicketParas,
            bool isForSign)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (strTicketParas.Length > 0)
                strTicketParas.Append(separator);

            strTicketParas.Append(name).Append("=").Append(isForSign ? value : value.UrlEncode());
        }

        #endregion

    }
}
