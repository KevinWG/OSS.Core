#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion


using OSS.Core.Domain;

namespace OSS.Core.Portal.Domain
{
    public class UserInfoMo : UserBasicMo
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string? pass_word { get; set; }
    }
    public class UserBasicMo : BaseOwnerMo<long>
    {
        /// <summary>
        ///  昵称
        /// </summary>
        public string? nick_name { get; set; }

        ///// <summary>
        /////  注册类型
        ///// </summary>
        //public RegLoginType reg_type { get; set; }

        /// <summary>
        ///  头像信息
        /// </summary>
        public string? avatar { get; set; }

        /// <summary>
        ///  邮件地址
        /// </summary>
        public string? email { get; set; }

        /// <summary> 
        ///  手机号
        /// </summary>
        public string? mobile { get; set; }

        /// <summary>
        ///  用户状态
        /// </summary>
        public UserStatus status { get; set; }
    }
}
