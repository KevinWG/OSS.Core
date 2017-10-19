#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 微信授权处理类
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
using OSS.Core.Domains.Sns.Oauth.Mos;
using OSS.Core.Services.Sns.Oauth.Handlers.Extention;
using OSS.SnsSdk.Oauth.Wx;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth.Handlers
{
    /// <summary>
    /// 微信授权处理类
    /// </summary>
    internal class WxOauthHander : BaseOauthHander<WxOauthHander>
    {
        private static readonly WxOauthApi _api = new WxOauthApi();

        /// <inheritdoc />
        public override ResultMo<string> GetOauthUrl(string redirectUrl, string state,
            AuthClientType type)
        {
            var url = _api.GetAuthorizeUrl(redirectUrl, state, type);
            return new ResultMo<string>(url);
        }
        
        /// <summary>
        ///  设置上下文方法
        /// </summary>
        public override void SetCOntextConfig(AppAuthorizeInfo appInfo)
        {
            // todo 多租户配置设置
            // WxOauthConfigProvider.SetContextConfig(config);
        }

        /// <inheritdoc />
        public override async Task<ResultMo<OauthAccessTokenMo>> GetOauthTokenAsync(string code, string state)
        {
            return (await _api.GetOauthAccessTokenAsync(code)).ConvertToComMo();
        }


        /// <inheritdoc />
        public override async Task<ResultMo<OauthUserMo>> GetOauthUserAsync(string accessToken,string appUserId )
        {
            return (await _api.GetWxOauthUserInfoAsync(accessToken, appUserId)).ConvertToComMo();
        }
    }
}
