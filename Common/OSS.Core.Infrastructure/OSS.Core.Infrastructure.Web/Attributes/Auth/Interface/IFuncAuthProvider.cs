using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Interface
{
    /// <summary>
    /// 功能权限判断接口
    /// </summary>
    public interface IFuncAuthProvider
    {
        /// <summary>
        ///  校验功能权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="identity"></param>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        Task<Resp> FuncAuthorize(HttpContext context, UserIdentity identity, AskUserFunc funcCode);
    }
}
