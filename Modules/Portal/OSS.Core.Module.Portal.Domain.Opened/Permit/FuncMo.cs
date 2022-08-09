
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public class FuncMo : BaseOwnerAndStateMo<long>
    {
        /// <summary>
        /// 权限码
        /// </summary>
        public string code { get; set; } = default!;

        /// <summary>
        ///  父级权限码
        /// </summary>
        public string? parent_code { get; set; }

        /// <summary>
        ///  标题
        /// </summary>
        public string title { get; set; } = default!;
    }
}
