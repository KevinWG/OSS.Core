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

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.WebSite.AppCodes.Filters
{
    /// <summary>
    ///   请求相关的系统信息
    ///  如果是App内嵌，免登录
    /// </summary>
    internal class SysAuthInfoMiddleware:BaseMiddlewaire
    {
        private readonly RequestDelegate _next;
        private static readonly string _appVersion;
        private static readonly string _appSource;

        static SysAuthInfoMiddleware()
        {
            var appConfig = ConfigUtil.GetSection("AppConfig");

            _appVersion = appConfig.GetSection("AppVersion").Value;// AppVersion.Value;
            _appSource= appConfig.GetSection("AppSource").Value;
        }

        public SysAuthInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            if (MemberShiper.AppAuthorize != null)
            {
                await _next.Invoke(context);
                return;
            }

            SysAuthorizeInfo sysInfo = null;

            //  这里是为了兼容App内部嵌套h5页面，使用App的授权信息
            string auticketStr = context.Request.Headers[GlobalKeysUtil.AuthorizeTicketName];
            if (!string.IsNullOrEmpty(auticketStr))
            {
                sysInfo=new SysAuthorizeInfo();
                sysInfo.FromSignData(auticketStr);
            }

            //  如果不是App访问，添加Web相关系统信息
            if (sysInfo==null)
            {
                sysInfo = new SysAuthorizeInfo
                {
                    Token = context.Request.Cookies[GlobalKeysUtil.UserCookieName],
                    DeviceId = "WEB"
                };
                // todo 给 webbrowser 赋值 
            }

            sysInfo.AppVersion = _appVersion;
            sysInfo.AppSource = _appSource;

            if (string.IsNullOrEmpty(sysInfo.IpAddress))
                sysInfo.IpAddress = GetIpAddress(context);

            MemberShiper.SetAppAuthrizeInfo(sysInfo);
            await _next.Invoke(context);
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
        internal static IApplicationBuilder UseSysAuthInfoMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SysAuthInfoMiddleware>();
        }
    }
    

    /// <summary>
    ///  中间件基类
    /// </summary>
    internal class BaseMiddlewaire
    {
        private static readonly string notFoundPage = ConfigUtil.GetSection("Authorize:NotFoundUrl").Value;
        
        /// <summary>
        ///   结束请求
        /// </summary>
        /// <param name="context"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        protected static async Task ResponseEnd(HttpContext context, ResultMo res)
        {
            if (IsAjax(context))
            {
                ClearCacheHeaders(context.Response);
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync($"{{\"ret\":{res.ret},\"message\":\"{res.msg}\"}}");
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Redirect;
                context.Response.Redirect(res.IsResultType(ResultTypes.ObjectNull)
                    ? notFoundPage
                    : string.Concat("/un/error?ret=", res.ret,"&message=",res.msg.UrlEncode()));
            }
        }

        /// <summary>
        ///  清理Response缓存
        /// </summary>
        /// <param name="httpResponse"></param>
        private static void ClearCacheHeaders(HttpResponse httpResponse)
        {
            httpResponse.Headers["Cache-Control"] = "no-cache";
            httpResponse.Headers["Pragma"] = "no-cache";
            httpResponse.Headers["Expires"] = "-1";
            httpResponse.Headers.Remove("ETag");
        }

        private static bool IsAjax( HttpContext context)
        {
            return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }

}
