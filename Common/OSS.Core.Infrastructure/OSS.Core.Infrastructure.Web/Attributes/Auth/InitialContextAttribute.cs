using System;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    public class InitialContextAttribute : Attribute, IAuthorizationFilter,IOrderedFilter
    {
        public int Order => -10000;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var appInfo = AppReqContext.Identity;
            if (appInfo == null)
            {
                AppWebInfoHelper.InitialDefaultAppIdentity(context.HttpContext);
            }
        }
    }
}
