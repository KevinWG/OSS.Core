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

using System;
using OSS.Common.Authrization;
using OSS.Common.Extention;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Mos;

namespace OSS.Core.Domains.Members
{
    /// <summary>
    ///  用户授权Token信息
    /// </summary>
    public class OauthAccessTokenMo : BaseMo
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

        /// <summary>
        ///  租户Id
        /// </summary>
        public long tenant_id { get; set; }
    }

    /// <summary>
    ///  第三方授权用户基础信息
    /// </summary>
    public class OauthUserMo : OauthAccessTokenMo
    {
        /// <summary>
        ///  用户Id
        /// </summary>
        public long user_id { get; set; }

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
        public SocialPaltforms platform { get; set; }

        /// <summary>
        ///  头像
        /// </summary>
        public string head_img { get; set; }
    }


    public static class OauthUserMaps
    {
        /// <summary>
        ///  通过从社交平台拿回来信息重新赋值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void SetFromSocial(this OauthUserMo target, OauthUserMo source)
        {
            var appInfo = MemberShiper.AppAuthorize;

            SetTokenInfo(target, source);

            target.head_img = source.head_img;
            target.app_union_id = source.app_union_id;
            target.nick_name = source.nick_name;
            target.sex = source.sex;

            target.app_user_id = source.app_user_id;
            target.tenant_id = appInfo.TenantId.ToInt64();
            target.create_time = DateTime.Now.ToUtcSeconds();
           
        }



        /// <summary>
        /// 设置token相关的信息
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void SetTokenInfo(this OauthUserMo target, OauthAccessTokenMo source)
        {
            target.access_token = source.access_token;
            target.expire_date = source.expire_date;
            target.refresh_token = source.refresh_token;
        }
    }
}
