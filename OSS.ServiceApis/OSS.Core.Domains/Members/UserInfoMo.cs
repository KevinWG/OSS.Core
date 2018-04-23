#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

using System;
using OSS.Common.Authrization;
using OSS.Common.Extention;
using OSS.Core.Infrastructure.Mos;

namespace OSS.Core.Domains.Members
{
    public class UserInfoBigMo : UserInfoMo
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string pass_word { get; set; }

        /// <summary>
        ///  应用来源
        /// </summary>
        public string app_source { get; set; }

        /// <summary>
        /// 应用版本号
        /// </summary>
        public string app_version { get; set; }

        /// <summary>
        ///  租户Id
        /// </summary>
        public long tenant_id { get; set; }
    }

    public class UserInfoMo : BaseMo
    {
        /// <summary>
        ///  昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        ///  头像信息
        /// </summary>
        public string head_img { get; set; }

        /// <summary>
        ///  邮件地址
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///  手机号
        /// </summary>
        public string mobile { get; set; }

    }


    public static class UserInfoMoMaps
    {
        /// <summary>
        ///  BigMo转化为Mo
        ///    主要防止直接返回BigMo附带用户密码，来源渠道等字段
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public static UserInfoMo ConvertToMo(this UserInfoBigMo io)
        {
            var userInfo = new UserInfoMo
            {
                email = io.email,
                nick_name = io.nick_name,
                head_img =io.head_img,
                mobile = io.mobile,
                id = io.id,

                create_time = io.create_time,
                status = io.status
            };
            return userInfo;
        }

        /// <summary>
        /// 授权用户转化为系统用户信息
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public static UserInfoBigMo ConvertToBigMo(this OauthUserMo io)
        {
            var appInfo = MemberShiper.AppAuthorize;

            var user = new UserInfoBigMo
            {
                create_time = DateTime.Now.ToUtcSeconds(),
                app_source = appInfo.AppSource,
                tenant_id = appInfo.TenantId.ToInt64(),
                app_version = appInfo.AppVersion,
                head_img = io.head_img,

                nick_name = io.nick_name
            };

            return user;
        }
    }
}
