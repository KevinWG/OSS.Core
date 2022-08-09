using Newtonsoft.Json;
using OSS.Clients.MApp.Wechat;
using OSS.Clients.Platform.Wechat.User;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public static class SocialUserMaps
    {
        //#region Oauth的平台用户转化

        ///// <summary>
        /////  通过从社交平台拿回来信息重新赋值
        ///// </summary>
        ///// <param name="socialUser"></param>
        ///// <param name="oauthUser"></param>
        //public static void FormatByOauthUser(this SocialUserMo socialUser, OauthUser oauthUser)
        //{
        //    socialUser.access_token = oauthUser.access_token;
        //    socialUser.expire_date = oauthUser.expire_date;
        //    socialUser.refresh_token = oauthUser.refresh_token;

        //    socialUser.social_plat = oauthUser.social_plat.ToAppPlat();
        //    socialUser.app_union_id = oauthUser.app_union_id;
        //    socialUser.app_user_id = oauthUser.app_user_id;

        //    socialUser.head_img = oauthUser.head_img;
        //    socialUser.nick_name = oauthUser.nick_name;
        //    socialUser.sex = oauthUser.sex.ToSexType();

        //    socialUser.ext = JsonConvert.SerializeObject(oauthUser);
        //}

        ///// <summary>
        /////  Oauth信息转化为SocialUser
        ///// </summary>
        ///// <param name="oauthUser"></param>
        //public static SocialUserMo ToSocialUser(this OauthUser oauthUser, string socialAppId)
        //{
        //    var newUser = new SocialUserMo();

        //    newUser.FormatByOauthUser(oauthUser);
        //    newUser.InitialBaseFromContext();

        //    newUser.social_app_id = socialAppId;

        //    return newUser;
        //}

        //private static SexType ToSexType(this OauthSexType oauthSex)
        //{
        //    switch (oauthSex)
        //    {
        //        case OauthSexType.Female:
        //            return SexType.Female;
        //        case OauthSexType.Male:
        //            return SexType.Male;
        //        default:
        //            return SexType.UnKnow;
        //    }
        //}

        //public static AppPlatform ToAppPlat(this OauthPlatform oauthPlat)
        //{
        //    switch (oauthPlat)
        //    {
        //        case OauthPlatform.WeChat:
        //        case OauthPlatform.WeChatApp:
        //            return AppPlatform.Wechat;
        //        case OauthPlatform.AliPay:
        //            return AppPlatform.Ali;
        //        case OauthPlatform.Sina:
        //            return AppPlatform.Sina;
        //        default:
        //            return AppPlatform.Self;
        //    }
        //}

        //public static OauthPlatform ToOauthPlat(this AppPlatform oauthPlat)
        //{
        //    switch (oauthPlat)
        //    {
        //        case AppPlatform.Wechat:
        //            return OauthPlatform.WeChat;
        //        case AppPlatform.Ali:
        //            return OauthPlatform.AliPay;
        //        case AppPlatform.Sina:
        //            return OauthPlatform.Sina;
        //    }
        //    throw new ArgumentException("正在获取一个未实现的Oauth平台接口！");
        //}

        //#endregion

        #region 微信公众号,小程序的用户转化


        public static AddOrUpdateSocialReq ToSocialUserReq(this WechatUserInfoResp wechatUser, string appKey)
        {
            var socialUser = new AddOrUpdateSocialReq
            {
                head_img  = wechatUser.headimgurl,
                nick_name = wechatUser.nickname,

                app_union_id  = wechatUser.unionid,
                social_plat   = AppPlatform.Wechat,
                app_user_id   = wechatUser.openid,
                social_app_key = appKey,

                ext = JsonConvert.SerializeObject(wechatUser),
                sex = GetSexByWechatSex(wechatUser.sex)
            };

            return socialUser;
        }

        public static void FormatByWechatMappUser(this SocialUserMo socialUser, WechatMAppUserInfo wechatUser)
        {
            socialUser.app_union_id = wechatUser.unionId;

            socialUser.head_img = wechatUser.avatarUrl;
            socialUser.nick_name = wechatUser.nickName;
            socialUser.sex = GetSexByWechatSex(wechatUser.gender);
        }

        private static SexType GetSexByWechatSex(int wechatSex)
        {
            if (wechatSex == 1)
            {
                return SexType.Male;
            }
            else if (wechatSex == 2)
            {
                return SexType.Female;
            }
            else
            {
                return SexType.UnKnow;
            }
        }

        #endregion

    }
}
