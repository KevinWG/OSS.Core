#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 成员信息公用类 （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion
using OSS.Core.DomainMos.Members.Mos;

namespace OSS.Core.Services.Members
{
    internal class MemberManager
    {
        public static UserInfoMo GetUserInfo(long userId)
        {
            return new UserInfoMo();// todo  暂时先返回
        }
    }
}
