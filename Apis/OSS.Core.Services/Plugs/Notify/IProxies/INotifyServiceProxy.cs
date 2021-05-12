using System.Threading.Tasks;
using OSS.Core.Services.Plugs.Notify.Mos;

namespace OSS.Core.Services.Plugs.Notify.IProxies
{
    public interface INotifyServiceProxy
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isFromLogModule">【重要】是否来自日志模块，直接决定当前方法内部异常写日志的操作</param>
        /// <returns></returns>
        Task<NotifyResp> Send(NotifyReq msg);
    }
}
