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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Core.Infrastructure;
using OSS.Tools.Config;

namespace OSS.Core.WebApi.App_Codes.AuthProviders
{
    public class AppAuthProvider : IAppAuthProvider
    {
        /// <summary>
        ///   应用服务端签名模式，对应的票据信息的请求头名称
        /// </summary>
        public static string ServerSignModeHeaderName { get; set; } = "at-id";

        /// <summary>
        ///  用户token 对应的cookie名称（在请求源在浏览器模式下尝试从cookie中获取用户token
        /// </summary>
        public static string UserTokenCookieName { get; set; } = "u_cn";
        

        public Task<Resp> AppAuthorize(AppIdentity appInfo, HttpContext context)
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
                    var res = CheckAppSign(appInfo,context);
                    if (!res.IsSuccess())
                        return Task.FromResult(res);

                    break;
                default:
                    appInfo.app_id  = AppInfoHelper.AppId;
                    appInfo.app_ver = AppInfoHelper.AppVersion;
                    appInfo.UDID    = "WEB";
                    break;
            }
            
            if (appInfo.source_mode >= AppSourceMode.Browser)
            {
                if (context.Request.Cookies.TryGetValue(UserTokenCookieName, out string tokenVal))
                    appInfo.token = tokenVal;
            }
            return Task.FromResult(new Resp());
        }

        public static Resp CheckAppSign(AppIdentity appInfo,HttpContext context)
        {
            var authTicketStr = context.Request.Headers[ServerSignModeHeaderName];

            appInfo.FromTicket(authTicketStr);
            if (!AppInfoHelper.FormatAppIdInfo(appInfo))
                return new Resp(RespTypes.OperateFailed, "未知应用来源！");


            var key = ConfigHelper.GetSection("KnockAppSecrets:" + appInfo.app_id)?.Value;

            const int expireSecs = 60 * 60 * 2;
            return appInfo.CheckSign(key, expireSecs);
        }
    }
}