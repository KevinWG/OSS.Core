using OSS.Clients.SMS.HW;
using OSS.Common.Resp;

namespace OSS.Core.Module.Notify;

public class HwSmsHandler : INotifyHandler
{
    public async Task<NotifyResp> NotifyMsg(TemplateMo template, NotifyReq msg)
    {
        var req = new HWSendSmsReq()
        {
            sender        = template.channel_sender,
            template_code = template.channel_template_code,
            body_paras    = msg.body_paras?.Select(p => p.Value).ToList(),
            phone_nums    = msg.targets
        };

        return Convert(await req.SendAsync());
    }

    public static NotifyResp Convert(HwSendResp resp)
    {
        return resp.code == "000000"
            ? new NotifyResp()
            : new NotifyResp().WithResp(RespCodes.OperateFailed, resp.description);
    }
}