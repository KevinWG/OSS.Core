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
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members;
using OSS.Core.WebApi.Controllers.CoreApi.Member.Reqs;
using OSS.Core.WebApi.Controllers.Member;
using OSS.Core.WebApi.Filters;

namespace OSS.Core.WebApi.Controllers.CoreApi.Member
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

            return await service.LoginUser(req.name, req.pass_word, req.type);
        }

        #endregion

        #region 登录注册参数校验

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


            var validator = new DataTypeAttribute(
                req.type == RegLoginType.Mobile
                    ? DataType.PhoneNumber
                    : DataType.EmailAddress);

            return !validator.IsValid(req.name)
                ? new ResultMo(ResultTypes.ParaError, "请输入正确的手机或邮箱！")
                : new ResultMo();
        }

        #endregion

 
        #endregion


        #region  第三方用户授权


        #endregion
    }
}
