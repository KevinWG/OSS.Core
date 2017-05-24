using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebSite.Controllers
{
    public class UserController : BaseController
    {

        public IActionResult Login()
        {
            return View();
        }
    }
}