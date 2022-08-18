using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Notify;

/// <summary>
///  通知模板对外WebApi
/// </summary>
public class NotifyController : BaseNotifyController, IOpenedNotifyService
{
    private static readonly IOpenedNotifyService _service = new NotifyService();

    /// <summary>
    ///  发送模板消息
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [AppMeta(AppAuthMode.AppSign)]
    public Task<NotifySendResp> Send([FromBody]NotifySendReq msg)
    {
        return _service.Send(msg);
    }
}