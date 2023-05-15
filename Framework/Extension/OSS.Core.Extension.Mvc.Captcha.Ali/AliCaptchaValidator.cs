using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Tools.Http;
using System.Text;
using OSS.Common;

namespace OSS.Core.Extension.Mvc.Captcha;

/// <summary>
///  非授权用户访问安全检测服务（使用阿里云云盾
/// </summary>
internal class CaptchaValidator : ICaptchaValidator
{
    /// <summary>
    ///  检查
    /// </summary>
    /// <returns></returns>
    public async Task<Resp> Validate(HttpContext context)
    {
        var viStr = context.Request.Query["v_i_c"].ToString();

        if (string.IsNullOrEmpty(viStr))
        {
            return new Resp(RespCodes.ParaError, "验证码参数(v_i_c)异常！");
        }

        var paraArr = viStr.Split('|');
        if (paraArr.Length < 4)
        {
            return new Resp(RespCodes.ParaError, "验证码参数异常！");
        }

        var scene     = paraArr[0];
        var token     = paraArr[1];
        var sessionId = paraArr[2];
        var sig       = paraArr[3];

        var appInfo   = CoreContext.App.Identity;
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        if (AliCaptchaHelper.SecretProvider == null)
            throw new RespNotImplementException("未能通过AliCaptchaHelper.AppSecretProvider获取对应秘钥信息!");

        var secret = await AliCaptchaHelper.SecretProvider.Get();


        var queryString = GetReqBasicQueryString(secret, sessionId, scene, token, sig, appInfo?.client_ip, timestamp);
        var sign        = GetSign(secret.access_secret, queryString);

        var reqUrl = string.Concat("https://afs.aliyuncs.com/", "?", queryString, "&Signature=", sign);
        var osReq = new OssHttpRequest(reqUrl)
        {
            http_method = HttpMethod.Get
        };

        var respContentStr = await osReq.SendAsync().ReadContentAsStringAsync();

        var aliRes = JsonConvert.DeserializeObject<AliCheckResp>(respContentStr);
        if (aliRes == null)
            return new Resp(RespCodes.OperateFailed, "校验验证码失败!");


        return aliRes.code == "100"
            ? Resp.Success()
            : new Resp(RespCodes.OperateFailed, string.Concat(aliRes.msg, aliRes.Message));
    }

    private static string GetReqBasicQueryString( AliCaptchaSecret secret,  string sessionId, string scene, string token, string sig, string? ip,
                                                  string timestamp)
    {
        var nonce = NumHelper.RandomNum(8);

        var urlStr = new StringBuilder(); // 不要打乱顺序

        urlStr.Append("AccessKeyId=").Append(secret.access_key);
        urlStr.Append("&Action=AuthenticateSig");
        urlStr.Append("&AppKey=").Append(secret.app_key);
        urlStr.Append("&Format=JSON");

        urlStr.Append("&RemoteIp=").Append(ip);
        urlStr.Append("&Scene=").Append(scene);
        urlStr.Append("&SessionId=").Append(sessionId.SafeEscapeUriDataString());
        urlStr.Append("&Sig=").Append(sig.SafeEscapeUriDataString());

        urlStr.Append("&SignatureMethod=HMAC-SHA1");
        urlStr.Append("&SignatureNonce=").Append(nonce);
        urlStr.Append("&SignatureVersion=1.0");
        urlStr.Append("&Timestamp=").Append(timestamp.SafeEscapeUriDataString());

        urlStr.Append("&Token=").Append(token.SafeEscapeUriDataString());
        urlStr.Append("&Version=2018-01-12");

        return urlStr.ToString();
    }

    private static string GetSign(string accessSecret, string queryStr)
    {
        string waitSignData = string.Concat("GET&%2F&", queryStr.SafeEscapeUriDataString());
        var    sign         = HMACSHA.EncryptBase64(waitSignData, string.Concat(accessSecret, "&"));

        return sign.SafeEscapeUriDataString();
    }
}