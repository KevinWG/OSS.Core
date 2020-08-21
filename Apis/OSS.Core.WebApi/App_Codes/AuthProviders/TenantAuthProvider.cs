using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;

namespace OSS.Core.WebApi.App_Codes.AuthProviders
{
    public class TenantAuthProvider :  ITenantAuthProvider
    {
        public Task<Resp<TenantIdentity>> InitialAuthTenantIdentity(HttpContext context, AppIdentity appInfo)
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
