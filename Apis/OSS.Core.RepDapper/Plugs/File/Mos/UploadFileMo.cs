

using OSS.Core.Infrastructure.BasicMos;
using OSS.Core.Infrastructure.BasicMos.File;

namespace OSS.Core.RepDapper.Plugs.File.Mos
{
    public class UploadFileMo:BaseOwnerAndStateMo
    {
        /// <summary>
        ///  租户id
        /// </summary>
        public string t_tid { get; set; }

        /// <summary>
        ///   地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        ///  上传文件类型
        /// </summary>
        public UploadFileType type { get; set; }

        /// <summary>
        ///  上传文件大小
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string mime_type { get; set; }


        /// <summary>
        ///  仓储空间名称
        /// </summary>
        public string bucket { get; set; }
    }
}
