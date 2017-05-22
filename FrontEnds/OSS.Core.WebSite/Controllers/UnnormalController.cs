using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebSite.Controllers
{
    public class UnnormalController : Controller
    {
        public IActionResult notfound()
        {
            return View();
        }
    }
}