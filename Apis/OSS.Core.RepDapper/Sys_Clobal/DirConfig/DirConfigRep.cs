using OSS.Common.BasicMos.Enums;
using OSS.Common.BasicMos.Resp;
using System;
using System.Threading.Tasks;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Extensions;
using OSS.Core.RepDapper.Sys_Clobal.DirConfig.Mos;

namespace OSS.Core.RepDapper.Sys_Clobal.DirConfig
{
    public class DirConfigRep : BaseRep<DirConfigRep, DirConfigMo>
    {
        protected override string GetTableName()
        {
            return "sys_dir_config";
        }

        /// <summary>
        ///  更新key值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public Task<Resp> Update(string key, string vals)
        {
            string cacheKey = string.Concat(CoreCacheKeys.System_Dir_Config_ByKey, key);
            return Update(u => new { u.dir_vals }, w => w.dir_key == key, new { dir_vals = vals }).WithCacheClear(cacheKey);
        }

        /// <summary>
        ///  通过Key值获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<Resp<DirConfigMo>> GetByKey(string key)
        {
            string cacheKey = string.Concat(CoreCacheKeys.System_Dir_Config_ByKey, key);

            Func<Task<Resp<DirConfigMo>>> getfunc = () => Get(w => w.dir_key == key && w.status > CommonStatus.Deleted);
            return getfunc.WithCache(cacheKey, TimeSpan.FromHours(1));
        }

      

    }
}
