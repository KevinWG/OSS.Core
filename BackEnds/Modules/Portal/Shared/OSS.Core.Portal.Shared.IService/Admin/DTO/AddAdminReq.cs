using System.ComponentModel.DataAnnotations;
using OSS.Core.Portal.Domain.Admin;

namespace OSS.Core.Portal.Shared.IService
{
    public class AddAdminReq
    {
        [Required]
        public long id { get; set; }

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
                id = req.id
            };
            return infoMo;
        }
    }
}
