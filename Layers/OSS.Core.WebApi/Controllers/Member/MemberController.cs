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
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Services.Members;
using OSS.Core.WebApi.Controllers.Member;

namespace OSS.Core.WebApi.Controllers.CoreApi.Member
{
    public class MemberController : BaseMemberApiController
    {
        private static readonly MemberService service=new MemberService();

        /// <summary>
        ///   获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultMo<UserInfoMo>> GetCurrentUser()
        {
            return await service.GetUserInfo(MemberShiper.Identity.Id);
        }

        /// <summary>
        ///  获取当前管理员账号信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultMo<AdminInfoMo>> GetCurrentAdmin()
        {
            return await service.GetAdminInfo(MemberShiper.Identity.Id);
        }

    }
}
