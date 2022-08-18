
namespace OSS.Core.Module.Portal
{
    public class ChangeRolePermitReq
    {
        /// <summary>
        /// 新增的项
        /// </summary>
        public List<string>? add_items { get; set; }

        /// <summary>
        /// 删除的项
        /// </summary>
        public List<string>? delete_items { get; set; }
    }
}
