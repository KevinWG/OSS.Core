using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Services.Basic.Portal.IProxies;

namespace OSS.Core.WebApi.App_Codes.AuthProviders
{
    public class UserAuthProvider : IUserAuthProvider
    {
        public void FormatUserToken(HttpContext context, AppIdentity appinfo)
        {
            
        }

        public Task<Resp<UserIdentity>> InitialAuthUserIdentity(HttpContext context, AppIdentity appInfo)
       {
          return  InsContainer<IPortalServiceProxy>.Instance.GetAuthIdentity();
        }
    }
}
