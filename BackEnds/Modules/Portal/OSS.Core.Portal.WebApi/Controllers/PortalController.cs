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
using OSS.Core.Portal.Service;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Portal.WebApi.Controllers;
using OSS.Core.WebApis.App_Codes.AuthProviders;

namespace OSS.Core.WebApis.Controllers.Basic.Portal
{
    /// <summary>
    ///  认证授权门户
    /// </summary>
    [AllowAnonymous]
    [Route("b/[controller]/[action]")]
    public class PortalController : BaseController
    {
        private static readonly AuthService service = new AuthService();

        /// <summary>
        ///   获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<Resp<UserIdentity>> GetIdentity()
        {
            return service.GetIdentity();
        }

        /// <summary>
        ///  判断 手机号/邮箱  是否能够注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> CheckIfCanReg([FromBody] PortalNameReq req)
        {
            return  service.CheckIfCanReg(req);
        }

    

        #region   密码注册登录

        /// <summary>
        ///  密码注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> PwdReg( [FromBody] PortalPasswordReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdReg(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token);

            return tokenResp;
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> PwdLogin( [FromBody] PortalPasswordReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdLogin(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token);
            return tokenResp;
        }

        /// <summary>
        /// 管理员密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> PwdAdminLogin( [FromBody] PortalPasswordReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdAdminLogin(req);

            PortalWebHelper.SetCookie(Response, tokenResp.token);
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

        #region 辅助校验方法

        /// <summary>
        ///   正常登录时，验证实体参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private Resp CheckLoginModelState(PortalLoginBaseReq req)
        {
            return req.CheckNameType();
        }


        #endregion
    }
}