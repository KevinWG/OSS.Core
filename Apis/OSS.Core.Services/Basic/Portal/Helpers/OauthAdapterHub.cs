using System;
using OSS.Adapter.Oauth.Interface;
using OSS.Adapter.Oauth.Interface.Mos.Enums;
using OSS.Adapter.Oauth.WX;
using OSS.Common.BasicImpls;
using OSS.Common.BasicMos;

namespace OSS.Core.Services.Basic.Portal.Helpers
{
    /// <summary>
    ///  Oauth 对接适配中心
    ///     todo 待扩展其他平台
    /// </summary>
    public static class OauthAdapterHub
    {
        /// <summary>
        ///     获取处理Adapter
        /// </summary>
        /// <param name="plat">平台类型</param>
        /// <returns></returns>
        public static IOauthAdapter GetAdapter(OauthPlatform plat)
        {
            // todo 处理配置
            switch (plat)
            {
                case OauthPlatform.WeChat:
                    return GetWeChatAdapter();
            }
            throw new ArgumentException("未实现的Oauth授权平台");
        }

        public static IOauthAdapter GetWeChatAdapter(AppConfig config=null)
        {
            var wxAdapter = SingleInstance<WXOauthAdapter>.Instance;
            //if (config != null)
            //{
            //    wxAdapter.(config);
            //}
            return wxAdapter;
        }

    }
}
