using System.Collections.Generic;

namespace OSS.Core.Services.Basic.Permit.Reqs
{
    public class ChangeRoleFuncItemsReq
    {
        /// <summary>
        /// 新增的项
        /// </summary>
        public List<string> add_items { get; set; }
     
        /// <summary>
        /// 删除的项
        /// </summary>
        public List<string> delete_items { get; set; }
    } 
}
