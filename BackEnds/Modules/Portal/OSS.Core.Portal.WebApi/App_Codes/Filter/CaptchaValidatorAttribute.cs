//using System;
//using System.Net.Http;
//using System.Text;
//using Microsoft.AspNetCore.Mvc.Filters;
//using OSS.Common.Resp;
//using OSS.Core.Context;
//using OSS.Core.Context.Attributes;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using OSS.Common.BasicMos;
//using OSS.Common.Encrypt;
//using OSS.Common.Extension;
//using OSS.Common.Helpers;
//using OSS.Tools.Http;

//namespace OSS.Core.WebApis.App_Codes.Filter
//{
//    /// <summary>
//    ///    浏览器端行为验证码
//    ///     如果 来源应用（SourceMode） 是 AppSourceMode.AppSign 则可以不用验证
//    /// </summary>
//    public class CaptchaValidatorAttribute : BaseAuthAttribute
//    {
//        /// <summary>
//        ///  行为校验码校验
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            var appInfo = CoreContext.App.Identity;
//            var viStr   = context.HttpContext.Request.Query["v_i_c"].ToString();
            
//            if (string.IsNullOrEmpty(viStr))
//            {
//                if (appInfo.source_mode == AppSourceMode.AppSign)
//                {
//                    // 服务器间调用可以验证，交由前台服务处理
//                    return;
//                }

//                ResponseExceptionEnd(context, appInfo, new Resp(RespTypes.ParaError, "验证码参数异常！"));
//                return;
//            }

//            var paraArr = viStr.Split('|');
//            if (paraArr.Length < 4)
//            {
//                ResponseExceptionEnd(context, appInfo, new Resp(RespTypes.ParaError, "验证码参数异常！"));
//                return;
//            }

//            var scene = paraArr[0];
//            var token = paraArr[1];
//            var sessionId = paraArr[2];
//            var sig = paraArr[3];

//            var checkRes = await AliCaptchaProvider.Check(sessionId, scene, token, sig, appInfo.client_ip);
//            if (!checkRes.IsSuccess())
//            {
//                ResponseExceptionEnd(context, appInfo, checkRes.WithErrMsg($"接口验证失败({checkRes.msg})!"));
//                return;
//            }
//        }
//    }



//    /// <summary>
//    ///  非授权用户访问安全检测服务（使用阿里云云盾
//    /// </summary>
//    public static class AliCaptchaProvider
//    {
//        // 配置信息
//        private const string _appKey = "FFFF0N0000000000A1A2";
//        private readonly static AppSecret _appConfig = new AppSecret()
//        {
//            app_id = "LTAI5tRJixqpfeZBmh8rzwXn",
//            app_secret = "SZVeiDLDP6VfnsPOpnF5E1CEo60NOY"
//        };

//        /// <summary>
//        ///  检查
//        /// </summary>
//        /// <returns></returns>
//        public static async Task<Resp> Check(string sessionId, string scene, string token, string sig, string remoteIp)
//        {
//            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

//            var queryString = GetReqBasicQueryString(sessionId, scene, token, sig, remoteIp, timestamp);
//            var sign = GetSign(queryString);

//            var reqUrl = string.Concat("https://afs.aliyuncs.com/", "?", queryString, "&Signature=", sign);
//            var osReq = new OssHttpRequest(reqUrl)
//            {
//                http_method = HttpMethod.Get
//            };

//            string respContentStr = string.Empty;
//            using (var resp = await osReq.SendAsync())
//            {
//                respContentStr = await resp.Content?.ReadAsStringAsync();
//            }

//            var aliRes = JsonConvert.DeserializeObject<AliCheckResp>(respContentStr);
//            if (aliRes == null)
//            {
//                return new Resp(RespTypes.OperateFailed, "校验验证码失败!");
//            }

//            if (aliRes.code == "100")
//            {
//                return new Resp();
//            }

//            return new Resp(RespTypes.OperateFailed, string.Concat(aliRes.msg, aliRes.Message));
//        }

//        private static string GetReqBasicQueryString(string sessionId, string scene, string token, string sig, string ip,
//            string timestamp)
//        {
//            var nonce = NumHelper.RandomNum(8);

//            var urlStr = new StringBuilder();// 不要打乱顺序

//            urlStr.Append("AccessKeyId=").Append(_appConfig.app_id);
//            urlStr.Append("&Action=AuthenticateSig");
//            urlStr.Append("&AppKey=").Append(_appKey);
//            urlStr.Append("&Format=JSON");

//            urlStr.Append("&RemoteIp=").Append(ip);
//            urlStr.Append("&Scene=").Append(scene);
//            urlStr.Append("&SessionId=").Append(sessionId.SafeEscapeUriDataString());
//            urlStr.Append("&Sig=").Append(sig.SafeEscapeUriDataString());

//            urlStr.Append("&SignatureMethod=HMAC-SHA1");
//            urlStr.Append("&SignatureNonce=").Append(nonce);
//            urlStr.Append("&SignatureVersion=1.0");
//            urlStr.Append("&Timestamp=").Append(timestamp.SafeEscapeUriDataString());

//            urlStr.Append("&Token=").Append(token.SafeEscapeUriDataString());
//            urlStr.Append("&Version=2018-01-12");

//            return urlStr.ToString();
//        }

//        private static string GetSign(string queryStr)
//        {
//            string waitSignData = string.Concat("GET&%2F&", queryStr.SafeEscapeUriDataString());
//            var sign = HMACSHA.EncryptBase64(waitSignData, string.Concat(_appConfig.app_secret, "&"));

//            return sign.SafeEscapeUriDataString();
//        }
//    }


//    public class AliCheckResp
//    {
//        public string code { get; set; }

//        public string msg { get; set; }

//        /// <summary>
//        ///  错误消息
//        /// </summary>
//        public string Message { get; set; }
//    }
//}
