

using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    //public class RoleUserBigMo : RoleUserMo
    //{
    //    /// <summary>
    //    ///  角色名称
    //    /// </summary>
    //    public string r_name { get; set; }
    //}

    public class RoleUserMo : BaseOwnerAndStateMo<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long u_id { get; set; }
        
        /// <summary>
        ///  角色Id
        /// </summary>
        public long role_id { get; set; }
    }
}
