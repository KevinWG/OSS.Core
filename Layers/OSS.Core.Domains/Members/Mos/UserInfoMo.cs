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
        /// 应用版本号
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

        ///// <summary>
        /////   用户状态
        /////  测试序列化时，取得是父类的值
        ///// </summary>
        //public new MemberStatus status { get; set; }
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
                mobile = io.mobile,
                Id = io.Id,
                create_time = io.create_time,

                status = io.status
            };
            return userInfo;
        }
    }
}
