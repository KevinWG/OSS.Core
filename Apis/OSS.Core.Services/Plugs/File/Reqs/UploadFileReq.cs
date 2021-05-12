using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Services.Plugs.File.Reqs
{
    public class UploadFileReq
    {
        [Required(ErrorMessage = "分类不能为空")]
        public string category { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expires { get; set; }

        /// <summary>
        ///  对象key
        /// </summary>
        [Required(ErrorMessage = "存储对象key值不能为空")] public string key { get; set; }
        
        [Required(ErrorMessage = "请求签名不能为空")]
        public string sign { get; set; }
    }
}
