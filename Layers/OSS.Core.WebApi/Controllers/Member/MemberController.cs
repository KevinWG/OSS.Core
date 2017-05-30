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

using System.Threading.Tasks;
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
        public async Task<UserRegisteResp> Registe([FromBody]UserRegisteReq req)
        {
            if (!ModelState.IsValid)
                return new UserRegisteResp() {ret = (int) ResultTypes.ParaNotMeet, message = "请检查参数填写是否正确！"};

            var regRes =await service.RegisteUser(req.value, req.pass_code, req.reg_type, MemberShiper.AppAuthorize);
            if (!regRes.IsSuccess())
                return regRes.ConvertToResult<UserRegisteResp>();

            var tokenRes = MemberTokenUtil.AppendToken(MemberShiper.AppAuthorize.AppSource, regRes.data.Id,
                MemberAuthorizeType.User);

            return tokenRes.IsSuccess() ?
                new UserRegisteResp() {token = tokenRes.data, user = regRes.data}
                : tokenRes.ConvertToResult<UserRegisteResp>();
        }

     
    }
}
