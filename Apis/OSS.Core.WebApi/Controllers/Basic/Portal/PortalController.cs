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

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Adapter.Oauth.Interface.Mos.Enums;
using OSS.Common.BasicMos.Resp;

using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.Services.Basic.Portal.Mos;
using OSS.Core.CoreApi.Controllers.Basic.Portal.Reqs;
using OSS.Core.Infrastructure.Const;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.WebApi.App_Codes.AuthProviders;
using OSS.Core.Context.Attributes;

namespace OSS.Core.CoreApi.Controllers.Basic.Portal
{
    [AllowAnonymous]
    [ModuleMeta(CoreModuleNames.Portal)]
    [Route("b/[controller]/[action]")]
    public partial class PortalController : BaseController
    {
        private static readonly PortalService service = new PortalService();

        /// <summary>
        ///   获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<Resp<UserIdentity>> GetMyself()
        {
            return PortalAuthHelper.GetMyself(Request);
        }

        /// <summary>
        ///  判断 手机号/邮箱  是否能够注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> CheckIfCanReg([FromBody] CheckRegNameReq req)
        {
            return !ModelState.IsValid
                ? Task.FromResult(GetInvalidResp())
                : service.CheckIfCanReg(req.reg_type, req.reg_name);
        }

        #region 短信动态码注册登录

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Resp> SendCode([FromBody] PortalLoginBaseReq req)
        {
            var checkRes = CheckNameType(req.name, req.type);
            if (!checkRes.IsSuccess())
                return checkRes;

            return await service.SendCode(req.name, req.type);
        }

        #region 【用户】动态码登录注册

        /// <summary>
        ///   验证码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> CodeReg([FromBody] PortalPasscodeReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes); //stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeReg(req.name, req.code, req.type, req.is_from_bind);

            PortalTokenFormat(tokenResp);

            return tokenResp;
        }

        /// <summary>
        ///   验证码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> CodeLogin([FromBody] PortalPasscodeReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);// stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeLogin(req.name, req.code, req.type, req.is_from_bind);

            PortalTokenFormat(tokenResp);

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
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes); //stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeRegOrLogin(req.name, req.code, req.type, req.is_from_bind);

            PortalTokenFormat(tokenResp);

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
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);// stateRes.ConvertToResultInherit<PortalTokenResp>();

            var tokenResp = await service.CodeAdminLogin(req.name, req.code, req.type, req.is_from_bind);

            PortalTokenFormat(tokenResp);

            return tokenResp;
        }

        #endregion

        #endregion

        #region   密码注册登录

        /// <summary>
        ///  密码注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> PwdReg([FromBody] PortalPasswordReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdReg(req.name, req.password, req.type, req.is_from_bind);

            PortalTokenFormat(tokenResp);

            return tokenResp;
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> PwdLogin([FromBody] PortalPasswordReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdLogin(req.name, req.password, req.type, req.is_from_bind);

            PortalTokenFormat(tokenResp);
            return tokenResp;
        }

        /// <summary>
        /// 管理员密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> PwdAdminLogin([FromBody] PortalPasswordReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return new PortalTokenResp().WithResp(stateRes);

            var tokenResp = await service.PwdAdminLogin(req.name, req.password, req.type, req.is_from_bind);
            PortalTokenFormat(tokenResp);
            return tokenResp;
        }

        /// <summary>
        ///  sourcemode 为 浏览器模式时才需要
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Resp Logout()
        {
            PortalAuthHelper.ClearCookie(Response);
            return new Resp();
        }

        private void PortalTokenFormat(PortalTokenResp tokenResp)
        {
            if (!tokenResp.IsSuccess())
            {
                return;
            }
            var appSourceMode = CoreAppContext.Identity.source_mode;
            if (appSourceMode >= AppSourceMode.Browser)
            {
                PortalAuthHelper.SetCookie(Response, tokenResp.token);
            }
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
            return !ModelState.IsValid
                ? new Resp(RespTypes.ParaError, GetInvalidMsg())
                : CheckNameType(req.name, req.type);
        }

        /// <summary>
        ///   检查验证登录类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Resp CheckNameType(string name, RegLoginType type)
        {
            if (string.IsNullOrEmpty(name))
                return new Resp(RespTypes.ParaError, "name 不能为空！");

            if (!Enum.IsDefined(typeof(RegLoginType), type))
                return new Resp(RespTypes.ParaError, "未知的账号类型！");

            var validator = new DataTypeAttribute(
                type == RegLoginType.Mobile
                    ? DataType.PhoneNumber
                    : DataType.EmailAddress);

            return !validator.IsValid(name)
                ? new Resp(RespTypes.ParaError, "请输入正确的手机或邮箱！")
                : new Resp();
        }
        #endregion
    }

    /// <summary>
    ///  第三方部分
    /// </summary>
    public partial class PortalController
    {
        #region 第三方账号直接登录注册【用户】

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="plat">平台</param>
        /// <param name="redirectUrl">重定向回跳地址</param>
        /// <param name="state">返回参数，自行编码</param>
        /// <param name="type">授权类型</param>
        /// <returns></returns>
        [HttpGet]
        public Task<StrResp> GetOauthUrl(SocialPlatform plat, string redirectUrl, string state, OauthClientType type)
        {
            return service.GetOauthUrl(plat, redirectUrl, state, type);
        }

        /// <summary>
        ///  系统用户主动绑定第三方平台
        /// 【当前MemberContext信息为 系统用户信息(User  or  admin)】
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> OauthBind(SocialPlatform plat, string code, string state)
        {
            return service.OauthBind(plat, code, state);
        }

        /// <summary>
        ///     通过第三方用户注册登录
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> OauthLogin(SocialPlatform plat, string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return new PortalTokenResp(RespTypes.ParaError, "code 不能为空！");

            var tokenResp = await service.OauthLogin(plat, code, state);
            PortalTokenFormat(tokenResp);

            return tokenResp;
        }

        /// <summary>
        ///   跳过系统登录注册页面，直接通过第三信息注册用户信息
        /// 【开启 用户选择是否跳过选项使用】
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> SkipWithReg()
        {
            var tokenResp = await service.SkipWithReg();
            PortalTokenFormat(tokenResp);

            return tokenResp;
        }

        #endregion

        #region 第三方账号登录功能【管理员】

        /// <summary>
        ///     通过第三方用户登录管理员账号
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PortalTokenResp> OauthAdminLogin(SocialPlatform plat, string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return new PortalTokenResp(RespTypes.ParaError, "code 不能为空！");

            var tokenResp = await service.OauthAdminLogin(plat, code, state);
            PortalTokenFormat(tokenResp);

            return tokenResp;
        }
        #endregion
    }
}