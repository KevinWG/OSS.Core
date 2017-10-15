using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Utils;
using OSS.Http.Mos;

namespace OSS.Core.WebSite.Controllers.Users
{
    public class OauthController : BaseController
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="plat">平台（参见ThirdPaltforms） 10-微信  20-支付宝  30-新浪 </param>
        /// <param name="state">回调附带参数</param>
        /// <param name="type">授权客户端类型 1-pc端web， 2-应用内浏览器（如公众号） 4-应用内静默授权</param>
        /// <returns></returns>
        public async Task<IActionResult> auth(int plat,string state, int type)
        {
            var redirectUrl = $"{m_CurrentDomain}/oauth/receive/{plat}";
            var authUrl = $"/oauth/getoauthurl?plat={plat}&redirectUrl={redirectUrl}&state={state}&type={type}";

            var urlRes =await ApiUtil.RestSnsApi<ResultMo<string>>(authUrl, null, HttpMothed.GET);
            if (urlRes.IsSuccess())
                return Redirect(urlRes.data);
            
            return Content(urlRes.msg);
        }

        /// <summary>
        /// 授权回调接收
        /// </summary>
        /// <param name="plat"></param>
        /// <returns></returns>
        public async Task<ResultMo> receive(int plat)
        {
            //  todo 获取用户信息
            return new ResultIdMo();
        }
    }
}
