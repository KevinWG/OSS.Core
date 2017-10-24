#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore领域模型层 —— 授权用户接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-19
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Members.Interfaces
{
    /// <summary>
    /// 授权用户接口
    /// </summary>
    public interface IOauthUserRep : IBaseRep
    {
        /// <summary>
        /// 通过应用的用户Id获取授权用户信息
        /// </summary>
        /// <param name="tenantId">租户Id</param>
        /// <param name="appUId">应用用户Id</param>
        /// <param name="plat">平台</param>
        /// <returns></returns>
        Task<ResultMo<OauthUserMo>> GetOauthUserByAppUserId(long tenantId, string appUId, SocialPaltforms plat);

        /// <summary>
        ///  更新授权用户和token信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResultMo> UpdateUserWithToken(OauthUserMo user);

        /// <summary>
        ///  修改授权绑定的用户Id
        /// </summary>
        /// <param name="oauthUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResultMo> UpdateUserIdByOauthId(long oauthUserId,long userId);
    }
}
