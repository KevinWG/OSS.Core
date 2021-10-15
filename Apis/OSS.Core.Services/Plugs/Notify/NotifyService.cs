using System;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Tools.Log;
using OSS.Core.Services.Plugs.Notify.SmsAdapters;

namespace OSS.Core.Services.Plugs.Notify
{
    public partial class NotifyService : INotifyServiceProxy
    {
        /// <summary>
        ///     发送通知消息
        /// </summary>
        /// <param name="msg">消息实体</param>
        /// <returns></returns>
        public async Task<NotifyResp> Send(NotifyReq msg)
        {
            string errCode;
            try
            {
                var tRes =await GetTemplateConfig(msg.t_code);
                if (!tRes.IsSuccess())
                    return new NotifyResp().WithResp(tRes);

                var template = tRes.data;

                //  todo 添加发送记录
                var handler = NotifyAdapterHub.GetNotifyHandler(template.notify_type, template.notify_channel);
                if (handler == null)
                    return new NotifyResp().WithResp(RespTypes.OperateFailed, "未知的发送平台！");

                return await handler.NotifyMsg(template, msg);
            }
            catch (Exception ex)
            {
                errCode = LogHelper.Error($"发送信息出错,错误信息：{ex.Message}", this.GetType().Name);
            }

            return new NotifyResp().WithResp(SysRespTypes.AppError, $"发送消息失败,错误码：{errCode}！");
        }


    }
}
