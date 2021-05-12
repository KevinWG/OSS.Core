using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSS.Core.Services.Plugs.File.Mos
{
    public class FileBucketItem
    {
        /// <summary>
        /// 仓储名称
        /// </summary>
        public string bucket_name { get; set; }

        /// <summary>
        /// 访问域名
        /// </summary>
        public string domain { get; set; }
    }
}
