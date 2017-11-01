#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 社会化配置模块
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-1
*       
*****************************************************************************/

#endregion


using OSS.Common.ComModels;
using OSS.SnsSdk.Oauth.Wx;
using OSS.SnsSdk.Official.Wx;

namespace OSS.Core.Services.Sns.Exchange
{
    /// <summary>
    ///  社会化模块配置设置类
    /// </summary>
    public static class SnsOauthConfigProvider
    {
        public static void RegisterDefaultWxConfig(string appId,string appSecret)
        {
            //  授权配置信息
            WxOauthConfigProvider.DefaultConfig=new AppConfig
            {
                AppId = appId,
                AppSecret = appSecret
            };
            //  公号配置信息
            WxOfficialConfigProvider.DefaultConfig =new AppConfig
            {
                AppId = appId,
                AppSecret = appSecret
            };
        }
    }
}
