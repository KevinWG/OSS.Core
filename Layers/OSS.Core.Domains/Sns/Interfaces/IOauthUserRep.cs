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
using OSS.Core.Domains.Sns.Mos;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Sns.Interfaces
{
    /// <summary>
    /// 授权用户接口
    /// </summary>
    public interface IOauthUserRep : IBaseRep
    {
        /// <summary>
        /// 通过应用的用户Id获取授权用户信息
        /// </summary>
        /// <param name="appUId">应用用户Id</param>
        /// <param name="plat">平台</param>
        /// <returns></returns>
        Task<ResultMo<OauthUserMo>> GetOauthUserByAppUId(string appUId, SocialPaltforms plat);
    }
}
