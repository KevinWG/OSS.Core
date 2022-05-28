//using Microsoft.AspNetCore.Http;
//using OSS.Common;
//using OSS.Common.Resp;
//using OSS.Core.Context;
//using OSS.Core.Context.Attributes;
//using System.Linq;
//using System.Threading.Tasks;
//using OSS.Core.Services.Basic.Permit.Proxy;

//namespace OSS.Core.WebApis.App_Codes.AuthProviders
//{
//    /// <inheritdoc />
//    public class FuncAuthProvider : IFuncAuthProvider
//    {
//        /// <inheritdoc />
//        public async Task<Resp> FuncAuthorize(HttpContext context, UserIdentity userIdentity, AskUserFunc askFunc)
//        {
//            var authType  = askFunc?.auth_type ?? PortalAuthorizeType.User;


//            var funcCode  = askFunc?.func_code;
//            var sceneCode = askFunc?.scene_code;

//            if (userIdentity.auth_type==PortalAuthorizeType.SuperAdmin)
//            {
//                userIdentity.data_level = FuncDataLevel.All;
//                return new Resp();
//            }

//            if (userIdentity.auth_type > authType)
//            {
//                switch (userIdentity.auth_type)
//                {
//                    case PortalAuthorizeType.SocialAppUser:
//                        return new Resp().WithResp(RespTypes.UserFromSocial, "需要绑定系统账号");
//                    case PortalAuthorizeType.UserWithEmpty:
//                        return new Resp().WithResp(RespTypes.UserIncomplete, "需要绑定手机号!");
//                }
//                return new Resp().WithResp(RespTypes.UserNoPermission, "权限不足!");
//            }
            
//            var checkRes = await CheckIfHaveFunc(funcCode, sceneCode);
//            if (checkRes.IsSuccess())
//            {
//                userIdentity.data_level = checkRes.data;
//            }
//            return checkRes;
//        }
        
//        /// <summary>
//        ///  判断登录用户是否具有某权限
//        /// </summary>
//        /// <param name="funcCode"></param>
//        /// <param name="sceneCode"></param>
//        /// <returns></returns>
//        public async Task<Resp<FuncDataLevel>> CheckIfHaveFunc( string funcCode, string sceneCode)
//        {
//            if (string.IsNullOrEmpty(funcCode))
//            {
//                return new Resp<FuncDataLevel>(FuncDataLevel.All);
//            }

//            var userFunc = await  InsContainer<IPermitService>.Instance.GetMyFuncs();
//            if (!userFunc.IsSuccess())
//                return new Resp<FuncDataLevel>().WithResp(userFunc);

//            var fullFuncCode = string.IsNullOrEmpty(sceneCode) ? funcCode : string.Concat(funcCode, ":", sceneCode);
//            var func         = userFunc.data.FirstOrDefault(f => f.func_code == fullFuncCode);

//            if (func == null)
//                return new Resp<FuncDataLevel>().WithResp(RespTypes.UserNoPermission, "无操作权限!");

//            return new Resp<FuncDataLevel>(func.data_level);
//        }

//    }
//}
