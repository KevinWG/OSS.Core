using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebSite.Controllers
{
    public class UnController : Controller
    {
        public IActionResult notfound()
        {
            return View();
        }

        public IActionResult Error(string msg)
        {
            ViewBag.Message = msg;
            return View();
        }
    }
}