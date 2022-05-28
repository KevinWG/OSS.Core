//using Microsoft.AspNetCore.Http;
//using OSS.Common;
//using OSS.Common.Resp;
//using OSS.Core.Context;
//using OSS.Core.Context.Attributes;
//using System.Threading.Tasks;
//using OSS.Core.Services.Basic.Portal.IProxies;

//namespace OSS.Core.WebApis.App_Codes.AuthProviders
//{
//    /// <inheritdoc />
//    public class UserAuthProvider : IUserAuthProvider
//    {
//        /// <inheritdoc />
//        public Task<Resp<UserIdentity>> GetIdentity(HttpContext context, AppIdentity appinfo)
//        {
//            return InsContainer<IPortalServiceProxy>.Instance.GetIdentity();
//        }
//    }
//}
