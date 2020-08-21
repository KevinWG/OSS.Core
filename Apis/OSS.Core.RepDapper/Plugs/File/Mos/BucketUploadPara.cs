using System.Collections.Generic;

namespace OSS.Core.Infrastructure.BasicMos.File
{
    public class BucketUploadPara
    {
        public BucketUploadPara()
        {
            paras = new Dictionary<string, string>(4);
        }

        public Dictionary<string, string> paras { get; set; }

        /// <summary>
        /// 上传地址
        /// </summary>
        public string upload_url { get; set; }
    }
}
