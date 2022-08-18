namespace OSS.Core.Module.Notify;

internal interface INotifyHandler
{
    Task<NotifySendResp> NotifyMsg(TemplateMo template, NotifySendReq msg);
}
