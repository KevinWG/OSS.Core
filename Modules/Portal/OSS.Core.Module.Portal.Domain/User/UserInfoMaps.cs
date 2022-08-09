using OSS.Core.Domain;
namespace OSS.Core.Module.Portal
{
    public static class UserInfoMaps
    {

        public static UserInfoMo ToUserInfo(this SocialUserMo socialUser)
        {
            var userMo = new UserInfoMo();

            userMo.avatar = socialUser.head_img;
            userMo.nick_name = socialUser.nick_name;

            userMo.FormatBaseByContext();

            userMo.owner_uid = 0; // 防止被TempSocial认证信息污染
            return userMo;
        }


    }
}
