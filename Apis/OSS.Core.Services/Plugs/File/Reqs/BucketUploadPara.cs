using System.Collections.Generic;

namespace OSS.Core.Services.Plugs.File.Reqs
{
    public class BucketUploadPara
    {
        public BucketUploadPara()
        {
            paras = new Dictionary<string, string>();
        }

        public IDictionary<string, string> paras { get; set; }

        /// <summary>
        /// 上传地址
        /// </summary>
        public string upload_address { get; set; }

        /// <summary>
        ///  访问地址
        /// </summary>
        public string access_url { get; set; }

    }
}
