
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
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OSS.Core.WebSite.AppCodes.Filters
{
    /// <summary>
    /// 用户授权验证
    /// </summary>
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(filter => filter is IAllowAnonymousFilter))
                return;




        }
    }
}
