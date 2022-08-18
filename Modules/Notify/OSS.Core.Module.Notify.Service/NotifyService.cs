using OSS.Common;
using OSS.Common.Resp;
using OSS.Tools.Log;

namespace OSS.Core.Module.Notify;

public class NotifyService : INotifyService
{
    private static readonly ITemplateService channelService = InsContainer<ITemplateService>.Instance;

    /// <summary>
    ///   发送通知消息
    /// </summary>
    /// <param name="msg">消息实体</param>
    /// <returns></returns>
    public async Task<NotifySendResp> Send(NotifySendReq msg)
    {
        string errCode;
        try
        {
            var tRes = await channelService.Get(msg.template_id);
            if (!tRes.IsSuccess())
                return new NotifySendResp().WithResp(tRes).WithErrMsg("获取发送模板信息失败!");

            var template = tRes.data;

            //  todo 添加发送记录
            var handler = NotifyHandlerHub.GetNotifyHandler(template.notify_type, template.notify_channel);

            return await handler.NotifyMsg(template, msg);
        }
        catch (Exception ex)
        {
            errCode = LogHelper.Error($"发送信息出错,错误信息：{ex.Message}", this.GetType().Name);
        }

        return new NotifySendResp().WithResp(SysRespCodes.AppError, $"发送消息失败,错误码：{errCode}！");
    }
}