using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using System.Text;

namespace OSS.Core.Context;

/// <summary>
///  来源(请求方)应用信息
/// </summary>
public class AppAuthReq
{
    #region 参与签名属性

    /// <summary>
    ///   【请求方】应用Id
    /// </summary>
    public string access_key { get; set; } = string.Empty;

    /// <summary>
    ///  【请求方】应用版本 (可选)
    /// </summary>
    public string app_ver { get; set; } = string.Empty;

    /// <summary>
    /// IP地址 (可选)
    /// </summary>
    public string? client_ip { get; set; }

    /// <summary>
    ///  请求跟踪编号 (唯一值必填)
    /// </summary>
    public string? trace_id { get; set; }

    /// <summary>
    /// 用户设备Id (可选)
    /// </summary>
    public string? UDID { get; set; }

    #endregion

    /// <summary>
    /// 时间戳  (必填)
    /// </summary>
    public long timestamp { get; set; }

    /// <summary>
    ///  sign标识
    /// </summary>
    public string sign { get; set; } = string.Empty;
}


/// <summary>
/// 授权信息扩展管理
/// </summary>
public static class AppAuthInfoExtension
{
    private const char _separateChar = '&';

    #region 字符串处理

    /// <summary>
    ///   从头字符串中初始化签名相关属性信息
    /// </summary>
    /// <param name="appAuthInfo"></param>
    /// <param name="ticket"></param>
    public static void FormatFromTicket(this AppAuthReq appAuthInfo, string? ticket)
    {
        if (string.IsNullOrEmpty(ticket)) return;

        var strArr = ticket.Split(_separateChar);
        foreach (var str in strArr)
        {
            if (string.IsNullOrEmpty(str)) continue;

            var keyValue = str.Split(new[] { '=' }, 2);
            if (keyValue.Length <= 1) continue;

            var val = keyValue[1].SafeUnescapeDataString();

            FormatProperty(appAuthInfo, keyValue[0], val);
        }
    }

    /// <summary>
    ///   格式化属性值
    /// </summary>
    /// <param name="appAuthInfo"></param>
    /// <param name="key"></param>
    /// <param name="val"></param>
    private static void FormatProperty(AppAuthReq appAuthInfo, string key, string val)
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

            case "trace_no":
                appAuthInfo.trace_id = val;
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

    #region 签名相关

    /// <summary>
    ///   检验是否合法
    /// </summary>
    /// <param name="appAuthInfo"></param>
    /// <param name="accessSecret"></param>
    /// <param name="signExpiredSeconds"></param>
    /// <param name="extSignData">参与签名的扩展数据（ 原签名数据 + "&amp;" + extSignData ）</param>
    /// <returns></returns>
    public static Resp CheckSign(this AppAuthReq appAuthInfo, string accessSecret, int signExpiredSeconds, string? extSignData = null)
    {
        if (appAuthInfo.timestamp <= 0 || string.IsNullOrEmpty(appAuthInfo.access_key))
            return new Resp(RespCodes.ParaError, "应用签名相关参数错误！");

        if (Math.Abs(DateTime.Now.ToUtcSeconds() - appAuthInfo.timestamp) > signExpiredSeconds)
            return new Resp(RespCodes.ParaExpired, "签名不在时效范围(请使用Unix Timestamp)！");

        var signData = ComputeSign(appAuthInfo, appAuthInfo.access_key, accessSecret, appAuthInfo.app_ver, extSignData);

        return appAuthInfo.sign == signData ? Resp.Success() : new Resp(RespCodes.ParaSignError, "签名错误！");
    }

    /// <summary>
    /// 生成签名后的字符串
    /// </summary>
    /// <param name="appReqInfo"></param>
    /// <param name="accessKey">当前应用来源</param>
    /// <param name="appVersion"></param>
    /// <param name="accessSecret"></param>
    /// <param name="extSignData">参与签名的扩展数据（ 原签名数据 + "&amp;" + extSignData ）</param>
    /// <returns></returns>
    public static string ToTicket(this AppAuthReq appReqInfo, string accessKey, string accessSecret, string appVersion,
        string? extSignData = null)
    {
        appReqInfo.timestamp = DateTime.Now.ToUtcSeconds();
        appReqInfo.sign = ComputeSign(appReqInfo, accessKey, accessSecret, appVersion, extSignData);

        var ticketStr = GetContent(appReqInfo, accessKey, appVersion,  false);
        AppendTicketBody(ticketStr, "sign", appReqInfo.sign, false);

        return ticketStr.ToString();
    }



    private static string ComputeSign(AppAuthReq appReqInfo, string accessKey, string accessSecret, string appVersion,
        string? extSignData)
    {
        var signContent = GetContent(appReqInfo, accessKey, appVersion,  true);
        if (!string.IsNullOrEmpty(extSignData))
            signContent.Append(_separateChar).Append(extSignData);

        return HMACSHA.EncryptBase64(signContent.ToString(), accessSecret);
    }

    /// <summary>
    ///   获取要加密签名的串
    /// </summary>
    /// <param name="appAuthInfo"></param>
    /// <param name="accessKey"></param>
    /// <param name="appVersion"></param>
    /// <param name="isForSign">是否签名校验时调用，签名校验时不进行url转义，否则需要转义</param>
    /// <returns></returns>
    private static StringBuilder GetContent(AppAuthReq appAuthInfo, string accessKey, string appVersion, bool isForSign)
    {
        var strTicketParas = new StringBuilder();

        AppendTicketBody(strTicketParas, "access_key", accessKey, isForSign);
        AppendTicketBody(strTicketParas, "app_ver", appVersion, isForSign);
        AppendTicketBody(strTicketParas, "client_ip", appAuthInfo.client_ip, isForSign);

        AppendTicketBody(strTicketParas, "timestamp", appAuthInfo.timestamp.ToString(), isForSign);
        AppendTicketBody(strTicketParas, "trace_no", appAuthInfo.trace_id, isForSign);

        AppendTicketBody(strTicketParas, "udid", appAuthInfo.UDID, isForSign);
        return strTicketParas;
    }

    /// <summary>
    ///   追加要加密的串
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="strTicketParas"></param>
    /// <param name="isForSign">是否参与加密字符串</param>
    private static void AppendTicketBody(StringBuilder strTicketParas, string name, string? value,  
        bool isForSign)
    {
        if (string.IsNullOrEmpty(value)) return;

        if (strTicketParas.Length > 0)
            strTicketParas.Append(_separateChar);

        strTicketParas.Append(name).Append('=').Append(isForSign ? value : value.SafeEscapeDataString());
    }

    #endregion

}
