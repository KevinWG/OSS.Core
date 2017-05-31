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

using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Members.Mos
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
        /// 
        /// </summary>
        public string app_version { get; set; }
    }

    public class UserInfoMo : BaseAutoMo
    {
        /// <summary>
        ///  昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        ///  邮件地址
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///  手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        ///   用户状态
        /// todo 待测试序列化和转化是否好使
        /// </summary>
        public new MemberStatus status { get; set; }
    }
}
