using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Core.Infrastructure.Helpers;
using OSS.CorePro.AdminSite.AppCodes;

namespace OSS.CorePro.TAdminSite.Apis
{
    public class ProxyApiController:BaseApiController
    {

        protected async Task<IActionResult> PostReqApi(string apiRoute)
        {
            string strContent;
            using (var reader = new StreamReader(Request.Body))
            {
                strContent = await reader.ReadToEndAsync();
            }
            var resStr =await RestApiHelper.PostStrApi(apiRoute, strContent);
            return Content(resStr, "application/json;", Encoding.UTF8);
        }

        protected async Task<IActionResult> GetReqApi(string apiRoute)
        {
            var resStr = await RestApiHelper.GetStrApi(apiRoute);
            return Content(resStr, "application/json;", Encoding.UTF8);
        }

    }
}
