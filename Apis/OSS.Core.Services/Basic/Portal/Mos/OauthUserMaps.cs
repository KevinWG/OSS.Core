using System;
using OSS.Adapter.Oauth.Interface.Mos;
using OSS.Adapter.Oauth.Interface.Mos.Enums;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;

namespace OSS.Core.Services.Basic.Portal.Mos
{
    public static class OauthUserMaps
    {
        /// <summary>
        ///  通过从社交平台拿回来信息重新赋值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void SetInfoFromSocial(this OauthUserMo target, OauthUser source)
        {
            var appInfo = AppReqContext.Identity;

            target.access_token = source.access_token;
            target.expire_date = source.expire_date;
            target.refresh_token = source.refresh_token;
            target.social_plat = source.social_plat.ToSocialPlat();

            target.head_img = source.head_img;
            target.app_union_id = source.app_union_id;
            target.nick_name = source.nick_name;
            target.sex = source.sex.ToSexType();

            target.app_user_id = source.app_user_id;
            target.add_time = DateTime.Now.ToUtcSeconds();
        }
        

      
        public static SexType ToSexType(this OauthSexType oauthSex)
        {
            switch (oauthSex)
            {
                case OauthSexType.Female:
                    return SexType.Female;
                case OauthSexType.Male:
                    return SexType.Male;
                default:
                    return SexType.UnKnow;
            }
        }

        public static SocialPlatform ToSocialPlat(this OauthPlatform oauthPlat)
        {
            switch (oauthPlat)
            {
                case OauthPlatform.WeChat:
                    return SocialPlatform.WeChat;
                case OauthPlatform.AliPay:
                    return SocialPlatform.AliPay;
                case OauthPlatform.Sina:
                    return SocialPlatform.Sina;
                case OauthPlatform.WeChatApp:
                    return SocialPlatform.WeChatApp;
                default:
                    return SocialPlatform.None;
            }
        }

        public static OauthPlatform ToOauthPlat(this SocialPlatform oauthPlat)
        {
            switch (oauthPlat)
            {
                case SocialPlatform.WeChat:
                    return OauthPlatform.WeChat;
                case SocialPlatform.AliPay:
                    return OauthPlatform.AliPay;
                case SocialPlatform.Sina:
                    return OauthPlatform.Sina;
                case SocialPlatform.WeChatApp:
                    return OauthPlatform.WeChatApp;
            }
            throw new ArgumentException("正在获取一个未实现的Oauth平台接口！");
        }
    }
}
