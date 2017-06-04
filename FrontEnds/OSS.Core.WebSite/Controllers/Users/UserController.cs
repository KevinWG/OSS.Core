

using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebSite.Controllers.Users
{
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}