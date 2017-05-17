#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 成员信息领域Service （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using OSS.Common.ComModels;
using OSS.Core.DomainMos;
using OSS.Core.DomainMos.Members.Interfaces;
using OSS.Core.DomainMos.Members.Mos;

namespace OSS.Core.Services.Members
{
    public class MemberService
    {
        /// <summary>
        /// 获取前台用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public  UserInfoMo GetUserInfo(long userId)
        {
            return MemberManager.GetUserInfo(userId);
        }

        public ResultMo<AdminInfoMo> GetAdminInfoByUserId(long userId)
        {
            return Rep<IAdminInfoRep>.Instance.Get<AdminInfoMo>(m => m.u_id == userId);
        }
    }
}
