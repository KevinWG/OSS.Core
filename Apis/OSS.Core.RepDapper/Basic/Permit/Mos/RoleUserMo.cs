
using OSS.Core.Infrastructure.BasicMos;
using OSS.Core.Infrastructure.BasicMos;

namespace OSS.Core.RepDapper.Basic.Permit.Mos
{
    public class RoleUserBigMo : RoleUserMo
    {
        /// <summary>
        ///  角色名称
        /// </summary>
        public string r_name { get; set; }
    }

    public class RoleUserMo : BaseOwnerAndStateMo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string u_id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string u_name { get; set; }

        /// <summary>
        ///  角色Id
        /// </summary>
        public string role_id { get; set; }
    }
}
