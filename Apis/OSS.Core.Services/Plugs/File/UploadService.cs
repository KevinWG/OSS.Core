

namespace OSS.Core.Services.Plugs.File
{
    public class UploadService
    {
       
        //public async Task<Resp> Notify(UploadFileMo mo,string token,string key)
        //{
        //    mo.InitialBaseFromContext();//不要放在下边，下边会给用户Id，和租户Id 根据实际信息赋值

        //    var res = AliOssHelper.FormatUploadMo(mo, token, key);
        //    if (!res)
        //       return new Resp(RespTypes.ParaError,"不正确的参数信息");

        //    // 1.  更新租户存储统计信息
        //    var storageUpdateRes = await FileStorageRep.Instance.UpdateUseSize(mo.owner_tid,mo.size);
        //    if (storageUpdateRes.IsSuccess())
        //    {
        //        // 2.  添加条目信息
        //        return await UploadFileRep.Instance.Add(mo);
        //    }
        //    return storageUpdateRes;
        //}
    }
}
