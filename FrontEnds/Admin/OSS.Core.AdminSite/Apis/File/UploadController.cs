using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth;

namespace OSS.CorePro.TAdminSite.Apis.File
{
    public class UploadController: ProxyApiController
    {
        /// <summary>
        ///  获取上传头像的参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserFuncCode(FuncCodes.None)]
        public Task<IActionResult> AvatarUploadPara()
        {
            const string url = "/p/file/GetUploadParas?category=avatar";
            return GetReqApi(url);
        }

    }
}
