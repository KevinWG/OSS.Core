

using Microsoft.AspNetCore.Mvc;
using OSS.Core.WebSite.AppCodes.Filters;

namespace OSS.Core.WebSite.Controllers
{
    [AuthorizeUser]
    public class BaseController : Controller
    {
    
    }
}