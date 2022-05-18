using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  模块授权认证
    /// </summary>
    public interface IModuleAuthProvider
    {
        /// <summary>
        ///  校验模块权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        Task<Resp> ModuleAuthorize(HttpContext context, string moduleName);
    }
}
