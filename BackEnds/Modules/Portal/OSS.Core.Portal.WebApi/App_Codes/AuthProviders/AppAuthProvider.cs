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

using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

namespace OSS.Core.WebApis.App_Codes.AuthProviders
{
    /// <inheritdoc />
    public class AppAuthProvider : IAppAuthProvider
    {
        /// <summary>
        ///  用户token 对应的cookie名称（在请求源在浏览器模式下尝试从cookie中获取用户token
        /// </summary>
        public static string UserTokenCookieName { get; set; } = "u_cn";

        /// <inheritdoc />
        public Task<Resp> AppAuthorize(AppIdentity appInfo, HttpContext context)
        {
            if (appInfo.source_mode != AppSourceMode.OutApp)
            {
                appInfo.source_mode = AppSourceMode.Browser;
            }
            
            appInfo.app_id = CoreContext.App.Self.AppId;
            appInfo.app_ver = CoreContext.App.Self.AppVersion;
            
            if (appInfo.source_mode >= AppSourceMode.Browser)
            {
                if (context.Request.Cookies.TryGetValue(UserTokenCookieName, out var tokenVal))
                    appInfo.token = tokenVal;
            }

            return Task.FromResult(new Resp());
        }

    }
}