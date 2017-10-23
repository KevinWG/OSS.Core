using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.WebSite.Controllers.Users.Mos;
using OSS.Http.Mos;

namespace OSS.Core.WebSite.Controllers.Users
{
    /// <summary>
    ///   用户模块
    /// </summary>
    [AllowAnonymous]
    public class PortalController : BaseController
    {
        #region 用户登录
        /// <summary>
        /// 用户登录页
        /// </summary>
        /// <param name="rurl"></param>
        /// <param name="state"> 用户状态，第三方授权绑定时使用 </param>
        /// <returns></returns>
        public IActionResult Login(string rurl,int state)
        {
            if (!string.IsNullOrEmpty(rurl))
            {
                Response.Cookies.Append(GlobalKeysUtil.UserReturnUrlCookieName, rurl);
            }
            return View();
        }

        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserRegLoginResp> Login(UserRegLoginReq req)
        {
            var loginRes =await RegOrLogin(req, "/portal/userlogin");

            return loginRes;
        }
        
        #endregion
        
        #region  用户注册

        public IActionResult Registe()
        {
            return View();
        }

        [HttpPost]
        public async Task<UserRegLoginResp> Registe(UserRegLoginReq req)
        {
            var regRes = await RegOrLogin(req, "/portal/userregiste");
            return regRes;
        }
        #endregion
        private async Task<UserRegLoginResp> RegOrLogin(UserRegLoginReq req, string apiUrl)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return stateRes.ConvertToResult<UserRegLoginResp>();

            var loginRes = await RestApiUtil.RestCoreApi<UserRegLoginResp>(apiUrl, req);
            if (!loginRes.IsSuccess()) return loginRes;

            Response.Cookies.Append(GlobalKeysUtil.UserCookieName, loginRes.token,
                new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.Now.AddDays(30) });

            loginRes.return_url = Request.Cookies[GlobalKeysUtil.UserReturnUrlCookieName] ?? "/";
            return loginRes;
        }


        /// <summary>
        ///   正常登录时，验证实体参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private ResultMo CheckLoginModelState(UserRegLoginReq req)
        {
            if (!ModelState.IsValid)
                return new ResultMo(ResultTypes.ParaError, GetVolidMessage());

            if (!Enum.IsDefined(typeof(RegLoginType), req.type))
                return new ResultMo(ResultTypes.ParaError, "未知的账号类型！");

            if (string.IsNullOrEmpty(req.pass_code)
                && string.IsNullOrEmpty(req.pass_word))
                return new ResultMo(ResultTypes.ParaError, "请填写密码或者验证码！");

            var validator = new DataTypeAttribute(
                req.type == RegLoginType.Mobile
                    ? DataType.PhoneNumber
                    : DataType.EmailAddress);

            return !validator.IsValid(req.name)
                ? new ResultMo(ResultTypes.ParaError, "请输入正确的手机或邮箱！")
                : new ResultMo();
        }
        #region 第三方用户授权
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="plat">平台（参见ThirdPaltforms） 10-微信  20-支付宝  30-新浪 </param>
        /// <param name="state">回调附带参数</param>
        /// <param name="type">授权客户端类型 1-pc端web， 2-应用内浏览器（如公众号） 4-应用内静默授权</param>
        /// <returns></returns>
        public async Task<IActionResult> auth(int plat, string state, int type)
        {
            var redirectUrl = $"{m_CurrentDomain}/oauth/receive/{plat}";
            var authUrl = $"/oauth/getoauthurl?plat={plat}&redirectUrl={redirectUrl}&state={state}&type={type}";

            var urlRes = await RestApiUtil.RestSnsApi<ResultMo<string>>(authUrl, null, HttpMothed.GET);
            if (urlRes.IsSuccess())
                return Redirect(urlRes.data);

            return Content(urlRes.msg);
        }

        private static readonly string loginUrl = ConfigUtil.GetSection("Authorize:LoginUrl")?.Value;

        /// <summary>
        /// 授权回调接收
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<IActionResult> receive(int plat, string code, string state)
        {
            var url = string.Concat("/portal/socialauth?plat=", plat, "&code=", code, "&state=", state);
            var userRes = await RestApiUtil.RestCoreApi<UserRegLoginResp>(url);

            if (!userRes.IsSuccess())
                return Redirect(string.Concat("/un/error?ret=", userRes.ret, "&message=", userRes.msg));

            Response.Cookies.Append(GlobalKeysUtil.UserCookieName, userRes.token,
                new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.Now.AddDays(30) });

            if (userRes.user.status >=0 )
            {
                var returnUrl = Request.Cookies[GlobalKeysUtil.UserReturnUrlCookieName] ?? "/";
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect(string.Concat(loginUrl, "?state=", userRes.user.status));
            }

      
        }
        #endregion

    }


}