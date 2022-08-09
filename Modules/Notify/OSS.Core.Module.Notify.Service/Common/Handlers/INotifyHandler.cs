namespace OSS.Core.Module.Notify;

internal interface INotifyHandler
{
    Task<NotifyResp> NotifyMsg(TemplateMo template, NotifyReq msg);
}
