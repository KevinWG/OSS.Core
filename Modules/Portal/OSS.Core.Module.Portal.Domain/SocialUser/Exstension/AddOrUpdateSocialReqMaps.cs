using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public static class AddOrUpdateSocialReqMaps
    {
        /// <summary>
        ///  转化为社交用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static SocialUserMo ToSocialUser(this AddOrUpdateSocialReq req)
        {
            var user = new SocialUserMo
            {
                social_plat   = req.social_plat,
                social_app_key = req.social_app_key,
                app_user_id   = req.app_user_id,
                app_union_id  = req.app_union_id,

                sex       = req.sex,
                nick_name = req.nick_name,
                head_img  = req.head_img
            };

            user.FormatBaseByContext();

            return user;
        }
        
    }
}
