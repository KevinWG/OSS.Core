using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Services.Plugs.Config.IProxies
{
    public interface IDirConfigServiceProxy
    {
        Task<Resp> RemoveDirConfig(string key);

        Task<Resp> SetDirConfig<TConfig>(string key, TConfig dirConfig) where TConfig : class, new();

        /// <summary>
        ///   获取字典配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="isFromLogModule">【重要】是否来自日志模块，直接决定当前方法内部异常写日志的操作</param>
        /// <returns></returns>
        Task<Resp<TConfig>> GetConfig<TConfig>(string key,bool isFromLogModule=false) where TConfig : class, new();
    }
}
