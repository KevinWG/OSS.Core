using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebSite.Controllers
{
    public class UnController : Controller
    {
        public IActionResult notfound()
        {
            
            return View();
        }

        public IActionResult Error(int? err_ret)
        {
            ViewBag.ErrRet = err_ret;
            return View();
        }
    }
}