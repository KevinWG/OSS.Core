using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Interface
{
    public interface IFuncAuthProvider
    {
        Task<Resp> CheckFuncPermission(HttpContext context, UserIdentity identity, string funcCode);
    }
}
