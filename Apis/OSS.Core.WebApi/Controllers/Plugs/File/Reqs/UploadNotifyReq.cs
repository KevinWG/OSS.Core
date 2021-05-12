using System;
using OSS.Common.BasicMos.Enums;
using OSS.Common.Extension;
using OSS.Common.Helpers;

namespace OSS.Core.CoreApi.Controllers.Plugs.File.Reqs
{
    //public class UploadNotifyReq
    //{
    //    /// <summary>
    //    ///   地址
    //    /// </summary>
    //    public string key { get; set; }

    //    /// <summary>
    //    ///  上传文件大小
    //    /// </summary>
    //    public int size { get; set; }

    //    /// <summary>
    //    /// 文件格式
    //    /// </summary>
    //    public string mime_type { get; set; }

    //    /// <summary>
    //    /// 回调中附带的token
    //    /// </summary>
    //    public string token { get; set; }

    //    /// <summary>
    //    ///  仓储空间名称
    //    /// </summary>
    //    public string bucket { get; set; }
    //}


    //public static class UploadNotifyReqMap
    //{
    //    /// <summary>
    //    ///  通知实体转化为上传实体对象
    //    /// </summary>
    //    /// <param name="req"></param>
    //    /// <returns></returns>
    //    public static UploadFileMo ConvertToMo(this UploadNotifyReq req)
    //    {
    //        var mo=new UploadFileMo();

    //        mo.mime_type = req.mime_type;
    //        mo.size = req.size;
    //        mo.bucket = req.bucket;
   

    //        mo.add_time = DateTime.Now.ToUtcSeconds();
    //        //mo.from_app_id = AppReqContext.Identity.app_id;
    //        mo.status = CommonStatus.Original;
    //        mo.id = NumHelper.SnowNum().ToString();

    //        return mo;
    //    }
    //}
}
