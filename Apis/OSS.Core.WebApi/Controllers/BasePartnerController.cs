using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.CoreApi.Controllers
{
    [AllowAnonymous]
    [Route("partner/[controller]/[action]/{id?}")]
    public class BasePartnerController : BaseController
    {


    }
}
