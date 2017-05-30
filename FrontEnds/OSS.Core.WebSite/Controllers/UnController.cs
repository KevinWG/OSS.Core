using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebSite.Controllers
{
    public class UnController : Controller
    {
        public IActionResult notfound()
        {
            
            return View();
        }

        public IActionResult error(int? err_ret)
        {
            //  todo 前台修改为通过Context获取
            ViewBag.ErrRet = err_ret;
            return View();
        }
    }
}