#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 第三方用户授权相关信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-19
*       
*****************************************************************************/

#endregion


using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Sns.Mos
{
    /// <summary>
    ///  用户授权Token信息
    /// </summary>
    public class OauthAccessTokenMo:BaseAutoMo
    {
        /// <summary>
        ///  应用的用户Id
        /// </summary>
        public string app_user_id { get; set; }

        /// <summary>
        /// 
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
    }

    /// <summary>
    ///  第三方授权用户基础信息
    /// </summary>
    public class OauthUserMo : OauthAccessTokenMo
    {
        /// <summary>
        /// 性别
        /// </summary>
        public Sex sex { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        ///  平台下统一用户Id
        /// </summary>
        public string app_union_id { get; set; }

        /// <summary>
        ///  应用平台
        /// </summary>
        public ThirdPaltforms platform { get; set; }
        
        /// <summary>
        ///  头像
        /// </summary>
        public string head_img { get; set; }
    }
}
