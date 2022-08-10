using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  应用授权提供者
    ///     todo 优化Attribute排除HttpContext参数，转移到基础Context类库
    /// </summary>
    public interface IAppAuthProvider
    {
        /// <summary>
        ///  应用授权检验
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        Task<IResp> Authorize(AppIdentity appInfo,HttpContext context);
    }
}
