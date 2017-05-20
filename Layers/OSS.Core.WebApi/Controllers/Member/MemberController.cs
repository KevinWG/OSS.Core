#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 成员相关接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members;
using OSS.Core.WebApi.Controllers.Member.Reqs;
using OSS.Core.WebApi.Filters;

namespace OSS.Core.WebApi.Controllers.Member
{
    [AuthorizeMember]
    public class MemberController : BaseApiController
    {
        private static readonly MemberService service=new MemberService();
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public UserRegisteResp Registe([FromBody]UserRegisteReq req)
        {
            if (!ModelState.IsValid)
                return new UserRegisteResp() {Ret = (int) ResultTypes.ParaNotMeet, Message = "请检查参数填写是否正确！"};

            var regRes = service.RegisteUser(req.value, req.pass_code, req.reg_type, MemberShiper.AppAuthorize);
            if (!regRes.IsSuccess)
                return regRes.ConvertToResult<UserRegisteResp>();

            var tokenRes = MemberTokenUtil.AppendToken(MemberShiper.AppAuthorize.AppSource, regRes.Data.Id,
                MemberAuthorizeType.User);

            return tokenRes.IsSuccess ?
                new UserRegisteResp() {token = tokenRes.Data, user = regRes.Data}
                : tokenRes.ConvertToResult<UserRegisteResp>();
        }

     
    }
}
