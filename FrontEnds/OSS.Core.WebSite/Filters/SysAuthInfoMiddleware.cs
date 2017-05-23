#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 请求需要的授权信息处理中间件
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-23
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.Authrization;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.WebSite.Filters
{
    /// <summary>
    ///   请求相关的系统信息
    /// </summary>
    internal class SysAuthInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private static string _appSource;
        private static string _appVersion;

        static SysAuthInfoMiddleware()
        {
            _appSource = ConfigUtil.GetSection("SysAuth:AppSource").Value;
            _appVersion = ConfigUtil.GetSection("SysAuth:AppVersion").Value;
        }


        public SysAuthInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        private const string authorizeTicket = "at_id";
        private const string tokenCookie = "ct_id";

        public async Task Invoke(HttpContext context)
        {
            if (MemberShiper.AppAuthorize != null)
            {
                await _next.Invoke(context);
                return;
            }

            SysAuthorizeInfo sysInfo = null;
            //  这里是为了兼容App嵌套h5页面，使用App的授权信息
            string auticketStr = context.Request.Headers[authorizeTicket];
            if (!string.IsNullOrEmpty(auticketStr))
            {
                sysInfo=new SysAuthorizeInfo();
                sysInfo.FromSignData(auticketStr);

                var secretKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource);
                if (!secretKeyRes.IsSuccess||!sysInfo.CheckSign(secretKeyRes.Data))
                {
                    context.Response.Redirect(string.Concat("/un/error?msg=","不正确的应用来源！"));
                    return;
                }
                sysInfo.OriginAppSource = sysInfo.AppSource;
            }

            //  如果不是App访问，添加Web相关系统信息
            if (sysInfo==null)
            {
                sysInfo = new SysAuthorizeInfo();
                sysInfo.Token = context.Request.Cookies["ct_id"];
                
                // todo appclient  
                sysInfo.DeviceId = "WEB";
               
            }

            CompleteAuthInfo(sysInfo,context);
            MemberShiper.SetAppAuthrizeInfo(sysInfo);

            await _next.Invoke(context);
        }

        /// <summary>
        ///   完善授权信息
        /// </summary>
        /// <param name="sysInfo"></param>
        /// <param name="context"></param>
        private static void CompleteAuthInfo(SysAuthorizeInfo sysInfo, HttpContext context)
        {
            if (string.IsNullOrEmpty(sysInfo.IpAddress))
                sysInfo.IpAddress = GetIpAddress(context);

            // todo webbrowser  
            sysInfo.AppSource = _appSource;
            sysInfo.AppVersion = _appVersion;
        }

        /// <summary>
        ///  获取IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetIpAddress(HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Forwarded-For"];
            return !string.IsNullOrEmpty(ipAddress) ? ipAddress : context.Connection.RemoteIpAddress.ToString();
        }
    }

    internal static class SysAuthInfoMiddlewareExtention
    {
        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SysAuthInfoMiddleware>();
        }
    }
}
