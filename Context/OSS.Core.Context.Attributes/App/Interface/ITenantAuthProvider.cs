using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  租户的授权实现接口
    /// </summary>
    public interface ITenantAuthProvider
    {
        /// <summary>
        ///  中间件初始化对应租户信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<TenantIdentity>> GetIdentity(HttpContext context,AppIdentity appInfo);
    }
}