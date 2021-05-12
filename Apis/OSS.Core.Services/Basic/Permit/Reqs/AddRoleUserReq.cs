using System.ComponentModel.DataAnnotations;
using OSS.Core.RepDapper.Basic.Permit.Mos;

namespace OSS.Core.Services.Basic.Permit.Reqs
{
    public class AddRoleUserReq
    {
        /// <summary>
        ///  角色ID
        /// </summary>
        [Required(ErrorMessage = "关联角色不能为空")] 
        public string role_id { get; set; }

        /// <summary>
        ///  用户Id
        /// </summary>
        [Required(ErrorMessage = "关联用户不能为空")]
        public string u_id { get; set; }

        /// <summary>
        ///  用户名称
        /// </summary>
        [Required(ErrorMessage = "关联用户不能为空")]
        public string u_name { get; set; }
    }
    
    public static class AddRoleUserReqMaps
    {
        public static RoleUserMo ToMo(this  AddRoleUserReq mo)
        {
            return new RoleUserMo()
            {
                role_id = mo.role_id,
                u_id = mo.u_id,
                u_name = mo.u_name
            };
        }
    }
}
