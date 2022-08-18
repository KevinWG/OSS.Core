using OSS.Common.Resp;

namespace OSS.Core.Module.Notify;

internal class SystemTestNotifyHandler : INotifyHandler
{
    public Task<NotifySendResp> NotifyMsg(TemplateMo template, NotifySendReq msg)
    {
        var body = msg.body_paras == null
           ? template.content
           : msg.body_paras.Aggregate(template.content,
               (current, p) => current.Replace(string.Concat("{", p.Key, "}"), p.Value));
        
        var res = new NotifySendResp().WithMsg(body);
        return Task.FromResult(res);
    }
}