using System.ComponentModel.DataAnnotations;
using OSS.Core.RepDapper.Basic.Portal.Mos;

namespace OSS.Core.WebApi.Controllers.Basic.Portal.Reqs
{
    public class AddAdminReq
    {
        [Required]
        public string u_id { get; set; }

        [Required]
        public string admin_name { get; set; }
    }

    public static class AddAdminReqMap
    {
        public static AdminInfoMo MapToAdminInfo(this AddAdminReq req)
        {
            var infoMo = new AdminInfoMo
            {
                admin_name = req.admin_name,
                u_id = req.u_id
            };
            return infoMo;
        }
    }
}
