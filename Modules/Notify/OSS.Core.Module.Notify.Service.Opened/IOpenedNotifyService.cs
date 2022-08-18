
namespace OSS.Core.Module.Notify
{
    public interface IOpenedNotifyService
    {
        /// <summary>
        ///  发送通知信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<NotifySendResp> Send(NotifySendReq msg);
    }
}
