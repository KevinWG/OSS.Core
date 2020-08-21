using System;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extention;
using OSS.Core.RepDapper.Plugs.File.Mos;

namespace OSS.Core.RepDapper.Plugs.File
{
    public class FileStorageRep : BaseTenantRep<FileStorageRep, TenantStorageAccountMo>
    {
        
        protected override string GetTableName()
        {
            return "p_file_storage";
        }
        /// <summary>
        ///  通过租户id获取租户的存储账号信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<Resp<TenantStorageAccountMo>> GetByTenantId(string tId)
        {
            return await Get(s => s.owner_tid == tId);
        }


        /// <summary>
        ///  更新使用容量
        /// </summary>
        /// <param name="owner_tid"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<Resp> UpdateUseSize(string owner_tid, int size)
        {
            var time = DateTime.Now.ToUtcSeconds();

            var para = new { size = size, m_time = time, owner_tid = owner_tid};
            return await Update(" cur_count=cur_count+1,cur_size=cur_size+@size,m_time=@m_time ", "where owner_tid=@owner_tid", para);
        }
    }
}
