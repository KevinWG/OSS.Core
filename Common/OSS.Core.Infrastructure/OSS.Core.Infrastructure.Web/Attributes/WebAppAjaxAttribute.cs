﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Extensions;

namespace OSS.Core.Infrastructure.Web.Attributes
{
    /// <summary>
    /// 接口请求模式验证
    /// </summary>
    public class WebFetchApiAttribute : BaseOrderAuthAttribute
    {
        public WebFetchApiAttribute()
        {
            p_Order = -99999;
            //p_IsWebSite = true;
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.IsFetchApi())
            {
                var res = new Resp(RespTypes.UnKnowOperate, "未知操作!");
                ResponseExceptionEnd(context, res);
            }
            return Task.CompletedTask;
        }

    }


}
