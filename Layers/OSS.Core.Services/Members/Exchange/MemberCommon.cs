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

using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Core.DomainMos;
using OSS.Core.DomainMos.Members.Interfaces;
using OSS.Core.DomainMos.Members.Mos;

namespace OSS.Core.Services.Members.Exchange
{
    /// <summary>
    ///  不同模块之间成员信息的共享类
    /// </summary>
    internal class MemberCommon
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<ResultMo<UserInfoMo>> GetUserInfo(long userId)
        {
            return await Rep<IUserInfoRep>.Instance.Get<UserInfoMo>();
        }
    }
}
