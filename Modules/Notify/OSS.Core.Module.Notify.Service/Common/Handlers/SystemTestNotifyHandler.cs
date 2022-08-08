using OSS.Common.Resp;

namespace OSS.Core.Module.Notify;

internal class SystemTestNotifyHandler : INotifyHandler
{
    public Task<NotifyResp> NotifyMsg(TemplateMo template, NotifyReq msg)
    {
        var body = msg.body_paras == null
           ? template.content
           : msg.body_paras.Aggregate(template.content,
               (current, p) => current.Replace(string.Concat("{", p.Key, "}"), p.Value));
        
        var res = new NotifyResp().WithMsg(body);
        return Task.FromResult(res);
    }
}