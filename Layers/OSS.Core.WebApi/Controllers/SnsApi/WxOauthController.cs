using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.SnsSdk.Oauth.Wx;
using OSS.SnsSdk.Oauth.Wx.Mos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.WebApi.Controllers.SnsApi
{
    public class WxOauthController : BaseSnsApiController
    {
        private static readonly WxOauthApi _OauthApi = new WxOauthApi();

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultMo<string> GetOauthUrl(string redirectUrl, string state, AuthClientType type)
        {
            var url = _OauthApi.GetAuthorizeUrl(redirectUrl, state, type);
            return new ResultMo<string>(url);
        }
        
    }
}
