#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 第三方用户授权相关信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-19
*       
*****************************************************************************/

#endregion


using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  第三方用户信息
    /// </summary>
    public class SocialUserSmallMo : BaseOwnerMo<long>
    {
        /// <summary>
        ///  应用平台
        /// </summary>
        public AppPlatform social_plat { get; set; }

        /// <summary>
        ///   社交平台Id
        /// </summary>
        public string social_app_key { get; set; }

        /// <summary>
        ///  应用的用户Id
        /// </summary>
        public string app_user_id { get; set; }

        /// <summary>
        ///  平台下统一用户Id
        /// </summary>
        public string app_union_id { get; set; }



        /// <summary>
        ///  状态
        /// </summary>
        public UserStatus status { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexType sex { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        ///  头像
        /// </summary>
        public string head_img { get; set; }


    }




    /// <summary>
    ///  第三方授权用户信息（附带token
    /// </summary>
    public class SocialUserMo : SocialUserSmallMo
    {
        /// <summary>
        ///   oauth 授权token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expire_date { get; set; }

        /// <summary>
        ///   第三方账号的其他扩展信息
        /// </summary>
        public string ext { get; set; }
    }
}
