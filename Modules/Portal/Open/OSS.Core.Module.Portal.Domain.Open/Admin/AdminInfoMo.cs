
#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 管理员实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion


using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  后台管理员实体 
    ///  管理员id 和 用户Id 相同
    /// </summary>
    public class AdminInfoMo : BaseOwnerMo<long>,IDomainStatus<AdminStatus>
    {
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string? admin_name { get; set; }

        /// <summary>
        /// 管理员图片
        /// </summary>
        public string? avatar { get; set; }

        /// <summary>
        ///  管理员类型   100.  超级管理员   0. 普通管理员
        /// </summary>
        public AdminType admin_type { get; set; }

        /// <summary>
        ///  状态
        /// </summary>
        public AdminStatus status { get; set; }
    }

}
