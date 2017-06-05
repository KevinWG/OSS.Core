
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户登录授权检验模块
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-23
*       
*****************************************************************************/

#endregion

using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.WebSite.AppCodes.Filters
{
    /// <summary>
    /// 用户授权验证
    /// </summary>
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        private static readonly string loginUrl = ConfigUtil.GetSection("Authorize:LoginUrl")?.Value;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(filter => filter is IAllowAnonymousFilter))
                return;

            var token = MemberShiper.AppAuthorize.Token;
            if (string.IsNullOrEmpty(token))
            {
                ReponseEnd(new ResultMo(ResultTypes.UnAuthorize, "用户未登录！"), context);
                return;
            }

            var userRes = UserCommon.GetCurrentUser().Result;
            if (!userRes.IsSuccess())
            {
                ReponseEnd(userRes, context);
                return;
            }

            MemberShiper.SetIdentity(new MemberIdentity() {Id = userRes.data.Id, MemberInfo = userRes.data});
        }

        private static void ReponseEnd(ResultMo res, AuthorizationFilterContext context)
        {
            if (IsAjax(context.HttpContext))
            {
                context.Result = new JsonResult(res);
            }
            else
            {
                if (res.IsResultType(ResultTypes.UnAuthorize))
                {
                    var rUrl = string.Concat(context.HttpContext.Request.Path,
                        context.HttpContext.Request.QueryString);
                    context.HttpContext.Response.Cookies.Append(GlobalKeysUtil.UserReturnUrlCookieName,
                       rUrl);
                    context.Result = new RedirectResult(loginUrl ?? "/");
                }
                else
                {
                    context.Result =
                        new RedirectResult(string.Concat("/un/error?ret=", res.ret, "&message=", res.message));
                }
            }
        }

        private static bool IsAjax(HttpContext context)
        {
            return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

    }
}
