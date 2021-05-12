using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Basic.Permit.Proxy;

namespace OSS.Core.CoreApi.App_Codes.AuthProviders
{
    public class FuncAuthProvider : IFuncAuthProvider
    {
        public async Task<Resp> CheckFuncPermission(HttpContext context, UserIdentity identity, string funcCode)
        {
            if (string.IsNullOrEmpty(funcCode)|| funcCode == ApiFuncCodes.None)
            {
                return new Resp();
            }

            return await InsContainer<IPermitService>.Instance.CheckIfHaveFunc(funcCode);
        }
    }
}
