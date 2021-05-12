
using OSS.Common.BasicImpls;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Core.Services.Plugs.Notify.NotifyAdapters.EmailHandlers;
using OSS.Core.Services.Plugs.Notify.SmsAdapters.Handlers;

namespace OSS.Core.Services.Plugs.Notify.SmsAdapters
{
    public static class NotifyAdapterHub
    {
        /// <summary>
        /// 获取通知处理handler
        /// </summary>
        /// <param name="type"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static INotifyHandler GetNotifyHandler(NotifyType type, NotifyChannel channel)
        {
            switch ((type, channel))
            {
                //case var prop when  prop.type== NotifyType.SMS&& prop.channel ==NotifyChannel.AliYun:
                //    return SingleInstance<AliSmsHandler>.Instance;

                case var prop when prop.type == NotifyType.Email && prop.channel == NotifyChannel.Self:
                    return SingleInstance<EmailHandler>.Instance;
            }
            return null;
        }
    }
}
