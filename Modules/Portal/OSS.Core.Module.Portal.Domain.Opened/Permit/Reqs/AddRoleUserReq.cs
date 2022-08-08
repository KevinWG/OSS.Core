using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Portal
{
    public class AddRoleUserReq
    {
        /// <summary>
        ///  角色ID
        /// </summary>
        [Required(ErrorMessage = "关联角色不能为空")]
        public long role_id { get; set; }

        /// <summary>
        ///  用户Id
        /// </summary>
        [Required(ErrorMessage = "关联用户不能为空")]
        public long u_id { get; set; }
    }

    public static class AddRoleUserReqMaps
    {
        public static RoleUserMo ToMo(this AddRoleUserReq mo)
        {
            return new RoleUserMo()
            {
                role_id = mo.role_id,
                u_id = mo.u_id
            };
        }
    }
}
