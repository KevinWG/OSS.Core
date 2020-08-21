using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Interface
{
    public interface IModuleAuthProvider
    {
        Task<Resp> CheckModulePermission(HttpContext context, string moduleName);
    }
}
