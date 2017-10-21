using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Sns.Oauth;
using OSS.SnsSdk.Oauth.Wx.Mos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.Core.WebApi.Controllers.SnsApi
{
    public class OauthController : BaseSnsApiController
    {
        private static readonly OauthService service = new OauthService();
        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="plat">平台</param>
        /// <param name="redirectUrl">重定向回跳地址</param>
        /// <param name="state">返回参数，自行编码</param>
        /// <param name="type">授权类型</param>
        /// <returns></returns>
        [HttpGet]
        public ResultMo<string> GetOauthUrl(ThirdPaltforms plat,string redirectUrl, string state, AuthClientType type)
        {
            return service.GetOauthUrl(plat, redirectUrl, state, type);
        }



    }
}
