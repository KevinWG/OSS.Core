﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;

namespace OSS.CorePro.TAdminSite.AppCodes.Initial
{
    public class TenantAuthProvider : ITenantAuthProvider
    {
        /// <summary>
        ///  中间件初始化对应租户信息
        /// </summary>
        /// <returns></returns>
        public Task<Resp<TenantIdentity>> CheckAndInitialIdentity(HttpContext context, AppIdentity appInfo)
        {
            var tenant = new TenantIdentity()
            {
                id   = "1",
                name = "OSSCore",
                logo = "http://img1.osscore.cn/static/oss_net.png"
            };
            return Task.FromResult(new Resp<TenantIdentity>(tenant));
        }


    }
}
