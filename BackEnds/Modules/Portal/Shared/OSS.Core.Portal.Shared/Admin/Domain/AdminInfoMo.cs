

using OSS.Core.Domain;

namespace OSS.Core.Portal.Shared.Admin.Domain
{

    /// <summary>
    ///  后台管理员实体 
    ///  管理员id 和 用户Id 相同
    /// </summary>
    public class AdminInfoMo : BaseOwnerMo<long>
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
