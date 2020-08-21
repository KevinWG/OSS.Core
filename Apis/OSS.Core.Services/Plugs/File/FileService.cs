using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;

using OSS.Core.Infrastructure.BasicMos;
using OSS.Core.Infrastructure.BasicMos.File;
using OSS.Core.RepDapper.Plugs.File;
using OSS.Core.RepDapper.Plugs.File.Mos;
using OSS.Core.Services.Plugs.File.Helpers;

namespace OSS.Core.Services.Plugs.File
{
    public class FileService:BaseService
    {
        /// <summary>
        ///  获取用户图片列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageListResp<UploadFileMo>> GetUploadImgs(SearchReq search)
        {
            var appInfo = AppReqContext.Identity;
            search.filters.Add("owner_tid", appInfo.tenant_id);

            search.filters.Add("owner_uid", UserContext.Identity.id);
            search.filters.Add("file_type", ((int)UploadFileType.Image).ToString());

            return await UploadFileRep.Instance.GetPageList(search);
        }
        
        public async Task<Resp> UploadNotify(UploadFileMo mo,string token,string key)
        {
            mo.InitialBaseFromContext();//不要放在下边，下边会给用户Id，和租户Id 根据实际信息赋值

            var res = AliOssHelper.FormatUploadMo(mo, token, key);
            if (!res)
               return new Resp(RespTypes.ParaError,"不正确的参数信息");

            // 1.  更新租户存储统计信息
            var storageUpdateRes = await FileStorageRep.Instance.UpdateUseSize(mo.owner_tid,mo.size);
            if (storageUpdateRes.IsSuccess())
            {
                // 2.  添加条目信息
                return await UploadFileRep.Instance.Add(mo);
            }
            return storageUpdateRes;
        }
        
        /// <summary>
        ///  获取上传参数信息
        /// </summary>
        /// <returns></returns>
        public async Task<Resp<BucketUploadPara>> GetUploadPara(string category)
        {
            var checkRes = await CheckStorageAccount();
            return checkRes.IsSuccess()
                ? AliOssHelper.GetUploadPara(UserContext.Identity.id, category)
                : new Resp<BucketUploadPara>().WithResp(checkRes);
        }


        private static readonly int defaultStorageSize = 1000000;

        /// <summary>
        ///  检查存储账号是否可用
        /// </summary>
        /// <returns></returns>
        internal static Task<Resp> CheckStorageAccount()
        {
            return Task.FromResult(new Resp());
            //var accountRes = await GetCurStorageAccount();
            //if (!accountRes.IsSuccess()) return accountRes;

            //var account = accountRes.data;
            //if (account.status < CommonStatus.Original || account.cur_size >= account.total_size)
            //{
            //    return new Resp(RespTypes.AuthFreezed, "存储账号容量不足，已被冻结");
            //}
            //return new Resp();
        }


        /// <summary>
        ///  获取存储账号查看是否可用
        /// </summary>
        /// <returns></returns>
        private static async Task<Resp<TenantStorageAccountMo>> GetCurStorageAccount()
        {
            var tId = AppReqContext.Identity.tenant_id;

            var accountRes = await FileStorageRep.Instance.GetByTenantId(tId);
            if (!accountRes.IsRespType(RespTypes.ObjectNull))
                return accountRes;

            var account = new TenantStorageAccountMo();

            account.InitialBaseFromContext();

            account.cur_size = 0;
            account.total_size = defaultStorageSize;

            var addRes = await FileStorageRep.Instance.Add(account);
            return addRes.IsSuccess()
                ? new Resp<TenantStorageAccountMo>(account)
                : new Resp<TenantStorageAccountMo>().WithResp(addRes);// addRes.ConvertToResult<TenantStorageAccountMo>();
        }


    }
}
