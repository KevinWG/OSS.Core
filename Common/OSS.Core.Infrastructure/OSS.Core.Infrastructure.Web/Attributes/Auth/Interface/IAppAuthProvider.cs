using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Interface
{
    /// <summary>
    ///  应用授权提供者
    /// </summary>
    public interface IAppAuthProvider
    {
        /// <summary>
        ///  应用授权检验
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appinfo"></param>
        /// <returns></returns>
        Task<Resp> AppAuthorize(AppIdentity appinfo,HttpContext context);
    }
}
