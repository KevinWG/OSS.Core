using OSS.Common.ComModels;
using OSS.SnsSdk.Oauth.Wx;
using OSS.SnsSdk.Official.Wx;

namespace OSS.Core.Services.Sns.Exchange
{
    /// <summary>
    ///  社会化模块配置设置类
    /// </summary>
    public static class SnsConfigProvider
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
