#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

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
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members;
using OSS.Core.WebApi.Controllers.Member.Reqs;

namespace OSS.Core.WebApi.Controllers.Member
{
    [AllowAnonymous]
    public class PortalController : BaseMemberApiController
    {
        private static readonly PortalService service = new PortalService();

        #region 用户登录注册

        #region 正常登录注册

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserTokenResp> UserRegiste([FromBody] UserRegLoginReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return stateRes.ConvertToResult<UserTokenResp>();

            return await service.RegisteUser(req.name,req.pass_word, req.pass_code, req.type);
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserTokenResp> UserLogin([FromBody] UserRegLoginReq req)
        {
            var stateRes = CheckLoginModelState(req);
            if (!stateRes.IsSuccess())
                return stateRes.ConvertToResult<UserTokenResp>();

            return await service.LoginUser(req.name, req.pass_word, req.pass_code, req.type);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResultMo> SendVertifyCode(string name, RegLoginType type)
        {
            var checkRes = CheckNameType(name, type);
            if (!checkRes.IsSuccess())
                return checkRes;

            return await service.SendVertifyCode(name, type);
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

            if (string.IsNullOrEmpty(req.pass_code)
                && string.IsNullOrEmpty(req.pass_word))
                return new ResultMo(ResultTypes.ParaError, "请填写密码或者验证码！");

            return CheckNameType(req.name,req.type);
        }

        /// <summary>
        ///   检查验证登录类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static ResultMo CheckNameType(string name, RegLoginType type)
        {
            if (string.IsNullOrEmpty(name))
                return new ResultMo(ResultTypes.ParaError, "name 不能为空！");

            if (!Enum.IsDefined(typeof(RegLoginType), type))
                return new ResultMo(ResultTypes.ParaError, "未知的账号类型！");

            var validator = new DataTypeAttribute(
                type == RegLoginType.Mobile
                    ? DataType.PhoneNumber
                    : DataType.EmailAddress);

            return !validator.IsValid(name)
                ? new ResultMo(ResultTypes.ParaError, "请输入正确的手机或邮箱！")
                : new ResultMo();
        }

        #endregion

 
        #endregion


        #region  第三方用户授权
        // 登录后直接 bind
        //  check whether thirduser had uid binded
        #endregion
    }
}
