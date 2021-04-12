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
using OSS.Common.BasicMos.Resp;

using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.Services.Basic.Portal.Mos;
using OSS.Core.WebApi.Controllers.Basic.Portal.Reqs;

namespace OSS.Core.WebApi.Controllers.Basic.Portal
{
    [AllowAnonymous]
    [ModuleName(CoreModuleNames.Portal)]
    [Route("b/[controller]/[action]/{id?}")]
    public partial class PortalController : BaseController
    {
        private static readonly PortalService service = new PortalService();

        /// <summary>
        ///   获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<Resp<UserIdentity>> GetAuthIdentity()
        {
            return service.GetAuthIdentity();
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
                ? Task.FromResult( GetInvalidResp()) 
                : service.CheckIfCanReg(req.reg_type,req.reg_name);
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

            return await service.CodeReg(req.name, req.code, req.type, req.is_from_bind);
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

            return await service.CodeLogin(req.name, req.code, req.type, req.is_from_bind);
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

            return await service.CodeRegOrLogin(req.name, req.code, req.type, req.is_from_bind);
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

            return await service.CodeAdminLogin(req.name, req.code, req.type, req.is_from_bind);
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

            return await service.PwdReg(req.name, req.password, req.type, req.is_from_bind);
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

            return await service.PwdLogin(req.name, req.password, req.type, req.is_from_bind);
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

            return await service.PwdAdminLogin(req.name, req.password, req.type, req.is_from_bind);
        }

        #endregion

        #region  第三方用户授权

       

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

    
}
