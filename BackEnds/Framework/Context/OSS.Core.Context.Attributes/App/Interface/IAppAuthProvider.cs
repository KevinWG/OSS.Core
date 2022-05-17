using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Context.Attributes
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
        /// <param name="appInfo"></param>
        /// <returns></returns>
        Task<Resp> AppAuthorize(AppIdentity appInfo,HttpContext context);
    }
}
