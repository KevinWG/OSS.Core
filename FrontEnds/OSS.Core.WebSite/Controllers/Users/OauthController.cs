using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Utils;
using OSS.Http.Mos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.WebSite.Controllers.Users
{
    public class OauthController : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> auth(ThirdPaltforms plat)
        {
            //sting
                 
            var urlRes =await ApiUtil.RestSnsApi<ResultMo<string>>("/oauth/getoauthurl", null, HttpMothed.GET);
            if (urlRes.IsSuccess())
            {
                return Redirect(urlRes.data);
            }
            return Content(urlRes.msg);
        }
    }
}
