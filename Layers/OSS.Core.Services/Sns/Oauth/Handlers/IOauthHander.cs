#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 授权处理接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-14
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Core.Domains.Sns.Mos;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth.Handlers
{
    internal interface IOauthHander
    {  
        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ResultMo<string> GetOauthUrl(string redirectUrl, string state, AuthClientType type);
        /// <summary>
        /// 通过授权回调code 获取授权用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<ResultMo<OauthAccessTokenMo>> GetOauthTokenAsync(string code, string state);

        /// <summary>
        /// 通过授权Token 获取授权用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        Task<ResultMo<OauthUserMo>> GetOauthUserAsync(string accessToken, string appUserId);

        /// <summary>
        /// 设置上下文配置信息
        /// </summary>
        void SetCOntextConfig(AppAuthorizeInfo appInfo);
    }
}