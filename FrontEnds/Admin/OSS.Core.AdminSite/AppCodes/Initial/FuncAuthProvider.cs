using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.CorePro.AdminSite.AppCodes;
using OSS.CorePro.TAdminSite.Apis.Permit.Helpers;

namespace OSS.CorePro.TAdminSite.AppCodes.Initial
{
    public class FuncAuthProvider : IFuncAuthProvider
    {
        public async Task<Resp> CheckFuncPermission(HttpContext context, UserIdentity identity, string funcCode)
        {
            if (string.IsNullOrEmpty(funcCode))
            {
                return new Resp<string>().WithResp(RespTypes.ObjectNull, "Api接口未关联FuncCode(需通过FuncCodeAttribute设置)");
            }

            if (funcCode == FuncCodes.None)
                return new Resp();

            return await FuncHelper.CheckAuthUserIfHaveFunc(funcCode);
        }
    }
}
