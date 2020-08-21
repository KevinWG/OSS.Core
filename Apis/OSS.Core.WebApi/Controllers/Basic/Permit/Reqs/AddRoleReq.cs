using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OSS.Core.RepDapper.Basic.Permit.Mos;

namespace OSS.Core.WebApi.Controllers.Basic.Permit.Reqs
{
    public class AddRoleReq
    {
        [Required(ErrorMessage = "角色名称不能为空！")]
        [StringLength(12, ErrorMessage = "名称不能超过12个字符！")]
        [RegularExpression("^[\u4e00-\u9fa5_a-zA-Z0-9]+$", ErrorMessage = "名称不得包含其他特殊字符！")]
        public string name { get; set; }
    }

    public class UpdateRoleReq: AddRoleReq
    {
        [Required]
        public string id { get; set; }
    }


    public static class AddRoleReqMaps
    {
        public static RoleMo ConvertToMo(this AddRoleReq req)
        {
            return new RoleMo()
            {
                name = req.name
            };
        }

        public static RoleMo ConvertToMo(this UpdateRoleReq req)
        {
            return new RoleMo()
            {
                id = req.id,
                name = req.name
            };
        }

    }

}
