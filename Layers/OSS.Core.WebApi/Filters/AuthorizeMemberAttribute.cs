using System;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Authrization;

namespace OSS.Core.WebApi.Filters
{
    public class AuthorizeMemberAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sysInfo = MemberShiper.AppAuthorize;

            //context.Result = new JsonResult(new ResultMo());
        }
    }
}
