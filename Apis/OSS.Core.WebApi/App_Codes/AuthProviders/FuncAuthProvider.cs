using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Basic.Permit.Proxy;

namespace OSS.Core.WebApi.App_Codes.AuthProviders
{
    public class FuncAuthProvider : IFuncAuthProvider
    {
        public async Task<Resp> FuncAuthorize(HttpContext context, UserIdentity identity, AskUserFunc askFunc)
        {
            if (string.IsNullOrEmpty(askFunc.func_code) || askFunc.func_code == CoreFuncCodes.None)
                return new Resp<FuncDataLevel>(FuncDataLevel.All);

            var checkRes =await InsContainer<IPermitService>.Instance.CheckIfHaveFunc(askFunc.func_code, askFunc.scene_code);
            if (checkRes.IsSuccess())
            {
                identity.data_level = checkRes.data;
            }

            return checkRes;
        }

    }
}
