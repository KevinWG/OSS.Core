#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 成员登录注册相关接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Extension.Mvc.Captcha;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  认证授权门户
    /// </summary>
    [AllowAnonymous]
    public class AuthController : BasePortalController,IAuthOpenService
    {
        private static readonly AuthService service = new();

        /// <summary>
        ///   获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IResp<UserIdentity>> GetIdentity()
        {
            return service.GetIdentity();
        }

        /// <summary>
        ///  判断 手机号/邮箱  是否能够注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<IResp> CheckIfCanReg([FromBody] PortalNameReq req)
        {
            return service.CheckIfCanReg(req);
        }

        #region 短信动态码注册登录

        /// <summary>
        /// 发送【短信/邮箱】验证码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [CaptchaValidator]
        public async Task<IResp> SendCode([FromBody] PortalNameReq req)
        {
            var app      = CoreContext.App.Identity;

            var checkRes = req.CheckNameType();
            if (!checkRes.IsSuccess())
                return checkRes;
            
            return await service.SendCode(req);
        }

        #region 【用户】动态码登录注册

        /// <summary>
        ///   验证码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> CodeLogin([FromBody] PortalPasscodeReq req)
        {
            var stateRes = req.CheckNameType();
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes); // stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeLogin(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token, req.remember);

            return tokenResp;
        }

        /// <summary>
        ///   验证码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> CodeRegOrLogin([FromBody] PortalPasscodeReq req)
        {
            var stateRes = req.CheckNameType();
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes); //stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeRegOrLogin(req);
            PortalWebHelper.SetCookie(Response, tokenResp.token, req.remember);

            return tokenResp;
        }

        #endregion

        #region 【管理员】动态码登录注册

        /// <summary>
        ///   验证码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> CodeAdminLogin([FromBody] PortalPasscodeReq req)
        {
            var stateRes = req.CheckNameType();
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes); // stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeAdminLogin(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token, req.remember);

            return tokenResp;
        }

        #endregion

        #endregion

        #region 密码注册登录

        /// <summary>
        ///  密码注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [CaptchaValidator]
        public async Task<PortalTokenResp> PwdReg([FromBody] PortalPasswordReq req)
        {
            var stateRes = req.CheckNameType();
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdReg(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token, req.remember);

            return tokenResp;
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [CaptchaValidator]
        public async Task<PortalTokenResp> PwdLogin([FromBody] PortalPasswordReq req)
        {
            var stateRes = req.CheckNameType();
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdLogin(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token, req.remember);
            return tokenResp;
        }

        /// <summary>
        /// 管理员密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [CaptchaValidator]
        public async Task<PortalTokenResp> PwdAdminLogin([FromBody] PortalPasswordReq req)
        {
            var stateRes = req.CheckNameType();
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdAdminLogin(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token, req.remember);
            return tokenResp;
        }

        /// <summary>
        ///  退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Resp Logout()
        {
            PortalWebHelper.ClearCookie(Response);
            return new Resp();
        }


        #endregion

   
    }
}