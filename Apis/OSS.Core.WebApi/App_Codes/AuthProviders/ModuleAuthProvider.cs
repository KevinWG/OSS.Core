using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;

namespace OSS.Core.CoreApi.App_Codes.AuthProviders
{
    public class ModuleAuthProvider : IModuleAuthProvider
    {
        public Task<Resp> CheckModulePermission(HttpContext context, string moduleName)
        {
            return Task.FromResult(new Resp());
        }
    }
}
