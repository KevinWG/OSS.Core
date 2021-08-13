using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Core.Infrastructure.Web.Attributes.Auth;

namespace OSS.Core.CoreApi.Controllers
{
    [AllowAnonymous]
    [AppPartnerMeta("Test")]
    [Route("[controller]/[action]/{id?}")]
    public class BasePartnerController : BaseController
    {
        [HttpGet]
        public string test()
        {
            return "test";
        }

    }



}
