using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;

namespace OSS.Core.WebApi.App_Codes.AuthProviders
{
    public class ModuleAuthProvider : IModuleAuthProvider
    {
        public Task<Resp> ModuleAuthorize(HttpContext context, string moduleName)
        {
            return Task.FromResult(new Resp());
        }
    }
}
