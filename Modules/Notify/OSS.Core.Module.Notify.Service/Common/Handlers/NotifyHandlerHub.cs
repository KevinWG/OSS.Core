using OSS.Common;

namespace OSS.Core.Module.Notify;

internal static class NotifyHandlerHub
{
    /// <summary>
    /// 获取通知处理handler
    /// </summary>
    /// <param name="type"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    public static INotifyHandler GetNotifyHandler(NotifyType type, NotifyChannel channel)
    {
        switch (type)
        {
            case NotifyType.SMS:
                return GetSMSNotifyHandler(channel);
            case NotifyType.Email:
                return GetEmailNotifyHandler(channel);
        }
        throw new NotImplementedException($"未实现的通知平台(NotifyChannel:{channel})");
    }


    private static INotifyHandler GetSMSNotifyHandler( NotifyChannel channel)
    {
        switch  (channel) 
        {
            case   NotifyChannel.HwCloud  :
                return SingleInstance<HwSmsHandler>.Instance;

            case  NotifyChannel.SystemPlatTest :
                return SingleInstance<SystemTestNotifyHandler>.Instance;
        }

        throw new NotImplementedException($"未实现的通知平台(NotifyChannel:{channel})");
    }



    private static INotifyHandler GetEmailNotifyHandler( NotifyChannel channel)
    {
        switch (channel)
        {
            case NotifyChannel.SystemPlat:
                return SingleInstance<EmailHandler>.Instance;

            case  NotifyChannel.SystemPlatTest :
                return SingleInstance<SystemTestNotifyHandler>.Instance;
        }

        throw new NotImplementedException($"未实现的通知平台(NotifyChannel:{channel})");
    }



}

