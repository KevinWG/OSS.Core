#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 应用交互Key辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

namespace OSS.Core
{
    /// <inheritdoc />
    public class AppAuthDefaultProvider : IAppAuthProvider
    {
        /// <summary>
        ///   应用服务端签名模式，对应的票据信息的请求头名称
        /// </summary>
        public static string ServerSignModeHeaderName { get; set; } = "at-id";

        /// <summary>
        ///  用户token 对应的cookie名称（在请求源在浏览器模式下尝试从cookie中获取用户token
        /// </summary>
        public static string UserTokenCookieName { get; set; } = "u_cn";

        
        /// <inheritdoc />
        public async Task<IResp> AppAuthorize(AppIdentity appInfo, HttpContext context)
        {
            if (appInfo.source_mode != AppSourceMode.OutApp)
            {
                appInfo.source_mode = context.Request.Headers.ContainsKey(ServerSignModeHeaderName)
                    ? AppSourceMode.AppSign
                    : AppSourceMode.Browser;
            }

            switch (appInfo.source_mode)
            {
                case AppSourceMode.AppSign:
                    var res = await CheckAppSign(appInfo, context);
                    if (!res.IsSuccess())
                        return res;
                    break;

                default:
                    appInfo.access_key = CoreContext.App.Self.AccessKey; // AppInfoHelper.AppId;
                    appInfo.app_ver    = CoreContext.App.Self.AppVersion;
                    appInfo.UDID       = "WEB";
                    break;
            }

            if (appInfo.source_mode >= AppSourceMode.Browser)
            {
                if (context.Request.Cookies.TryGetValue(UserTokenCookieName, out var tokenVal))
                    appInfo.token = tokenVal;
            }
            return Resp.DefaultSuccess;
        }
        
        private static readonly IResp _checkErrorRes = new Resp(RespCodes.OperateFailed, "未知应用来源！");


        private static async Task<IResp> CheckAppSign(AppIdentity appIdentity, HttpContext context)
        {
            var authTicketStr = context.Request.Headers[ServerSignModeHeaderName];

            appIdentity.FromTicket(authTicketStr);
            
            //if (!AppKeyHelper.FormatAppKeyInfo(appIdentity))
            //    return _checkErrorRes;

            throw new RespNotImplementException("获取应用秘钥验证未实现");
            
   

            const int expireSecs = 60 * 60 * 2;
            return appIdentity.CheckSign("xxxx秘钥（待实现）", expireSecs);
        }
    }
}