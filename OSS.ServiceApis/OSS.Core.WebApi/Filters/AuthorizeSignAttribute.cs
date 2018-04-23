#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 签名验证中间件
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.WebApi.Filters
{
    /// <summary>
    ///  签名验证中间件
    /// </summary>
    internal class AuthorizeSignAttribute : Attribute, IAuthorizationFilter
    {
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

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AppAuthorizeInfo sysInfo = null;
            var checkSign = !context.Filters.Any(filter => filter is AllowNoSignAttribute);
          
            if (checkSign)
            {
                string auticketStr = context.HttpContext.Request.Headers[GlobalKeysUtil.AuthorizeTicketName];
                if (string.IsNullOrEmpty(auticketStr))
                {
                    context.Result = new JsonResult(new ResultMo(ResultTypes.UnKnowSource, "未知应用来源"));
                    return;
                }

                sysInfo = new AppAuthorizeInfo();
                sysInfo.FromTicket(auticketStr);

                var secretKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource, sysInfo.TenantId);
                if (!secretKeyRes.IsSuccess())
                {
                    context.Result = new JsonResult(secretKeyRes);
                    return;
                }

                if (!sysInfo.CheckSign(secretKeyRes.data))
                {
                    context.Result = new JsonResult(new ResultMo(ResultTypes.ParaError, "非法应用签名！"));
                    return;
                }
            }

            if (sysInfo == null)
                sysInfo = new AppAuthorizeInfo();

            SetSystemAuthorizeInfo(sysInfo, context);
            MemberShiper.SetAppAuthrizeInfo(sysInfo);
        }
        
        private static readonly string _appSource = ConfigUtil.GetSection("AppConfig:AppSource")?.Value;
        private static readonly string _appVersion = ConfigUtil.GetSection("AppConfig:AppVersion")?.Value;

        private static void SetSystemAuthorizeInfo(AppAuthorizeInfo sysInfo, ActionContext context)
        {
            sysInfo.AppSource = _appSource;
            sysInfo.AppVersion = _appVersion;

            if (string.IsNullOrEmpty(sysInfo.IpAddress))
                sysInfo.IpAddress = GetIpAddress(context.HttpContext);

            // todo  设置浏览器等值
        }
    }

    public class AllowNoSignAttribute : Attribute, IFilterMetadata
    {

    }

}
