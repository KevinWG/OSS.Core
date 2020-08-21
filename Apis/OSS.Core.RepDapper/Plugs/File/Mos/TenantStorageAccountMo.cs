

using OSS.Core.Infrastructure.BasicMos;

namespace OSS.Core.RepDapper.Plugs.File.Mos
{
    public class TenantStorageAccountMo:BaseOwnerAndStateMo
    {
        /// <summary>
        ///  总大小
        /// </summary>
        public int total_size { get; set; }

        /// <summary>
        ///  当前大小容量
        /// </summary>
        public int cur_size { get; set; }
        
        /// <summary>
        ///  当前数量
        /// </summary>
        public int cur_count { get; set; }
    }
}
