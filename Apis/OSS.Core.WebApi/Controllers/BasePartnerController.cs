using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("partner/[controller]/[action]/{id?}")]
    public class BasePartnerController : ControllerBase
    {
    }
}
