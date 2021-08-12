using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Services.Basic.Permit.Proxy;

namespace OSS.Core.WebApi.App_Codes.AuthProviders
{
    public class FuncAuthProvider : IFuncAuthProvider
    {
        public async Task<Resp<FuncDataLevel>> CheckFunc(HttpContext context, UserIdentity identity, AskUserFunc askFunc)
        {
            if (string.IsNullOrEmpty(askFunc.func_code) || askFunc.func_code == CoreFuncCodes.None)
            {
                return new Resp<FuncDataLevel>(FuncDataLevel.All);
            }

            return await InsContainer<IPermitService>.Instance.CheckIfHaveFunc(askFunc.func_code, askFunc.query_code);
        }

    }
}
