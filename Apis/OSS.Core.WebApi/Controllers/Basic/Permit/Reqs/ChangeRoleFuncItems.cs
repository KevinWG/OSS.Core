using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.WebApi.Controllers.Basic.Permit.Reqs
{
    public class ChangeRoleFuncItems
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
