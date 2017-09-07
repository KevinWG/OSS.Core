using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Core.WebSite.AppCodes.Tools;

namespace OSS.Core.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var res= ApiUtil.PostApi<ResultMo>("/member/test").Result;
            return View();
        }

    }
}
