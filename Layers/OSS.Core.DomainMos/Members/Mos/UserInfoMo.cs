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

namespace OSS.Core.DomainMos.Members.Mos
{
    public class UserInfoBigMo : UserInfoMo
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string pass_word { get; set; }
    }

    public class UserInfoMo: BaseAutoMo
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
    }
}
