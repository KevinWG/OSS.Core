using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Core.WebSite.Controllers.Mos.Reqs;

namespace OSS.Core.WebSite.Controllers
{
    /// <summary>
    ///   ÓÃ»§Ä£¿é
    /// </summary>
    public class UserController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// ÓÊÏäµÇÂ¼
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> mobile_login(UserMobileLoginReq req)
        {
            await Task.Delay(100);
            return Json(new ResultMo());
        }
        



    }


}