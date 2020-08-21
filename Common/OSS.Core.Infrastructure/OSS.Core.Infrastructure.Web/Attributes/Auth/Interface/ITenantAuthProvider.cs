using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Interface
{
    public interface ITenantAuthProvider
    {
        /// <summary>
        ///  中间件初始化对应租户信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<TenantIdentity>> InitialAuthTenantIdentity(HttpContext context,AppIdentity appInfo);
    }
}