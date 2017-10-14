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

using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth.Handlers
{
    /// <summary>
    /// 授权处理接口
    /// </summary>
    public class BaseOauthHander
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
    }

    public class BaseOauthHander<HType>: BaseOauthHander where
        HType : BaseOauthHander, new()
    {
        private static HType _instance;
        public static HType Instance => _instance ?? (_instance = new HType());
    }
}