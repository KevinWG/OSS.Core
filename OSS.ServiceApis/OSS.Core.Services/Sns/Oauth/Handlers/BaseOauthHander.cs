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
using OSS.Common.ComModels.Enums;
using OSS.Core.Domains.Members;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth.Handlers
{
    internal class BaseOauthHander<HType> where
        HType : IOauthHander, new()
    {
        private static HType _instance;

        public static HType Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HType();
                return _instance;
            }
        }
    }

    /// <summary>
    /// 授权处理接口
    /// </summary>
    internal class NoneOauthHander :IOauthHander
    {
        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual ResultMo<string> GetOauthUrl(string redirectUrl, string state,
            AuthClientType type)
        {
            return new ResultMo<string>(ResultTypes.UnKnowSource, "未知应用平台！");
        }

        /// <summary>
        /// 设置上下文配置信息
        /// </summary>
        public virtual void SetCOntextConfig(AppAuthorizeInfo appInfo)
        {
        }

        /// <summary>
        /// 通过授权回调code 获取授权用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual async Task<ResultMo<OauthAccessTokenMo>> GetOauthTokenAsync(string code,string state)
        {
            return new ResultMo<OauthAccessTokenMo>(ResultTypes.UnKnowSource, "未知应用平台！");
        }


        /// <summary>
        /// 通过授权Token 获取授权用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        public virtual async Task<ResultMo<OauthUserMo>> GetOauthUserAsync(string accessToken, string appUserId)
        {
            return new ResultMo<OauthUserMo>(ResultTypes.UnKnowSource, "未知应用平台！");
        }

    }


}
