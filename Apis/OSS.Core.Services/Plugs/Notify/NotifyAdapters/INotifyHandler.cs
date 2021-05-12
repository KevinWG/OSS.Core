using System.Threading.Tasks;
using OSS.Core.Services.Plugs.Notify.Mos;

namespace OSS.Core.Services.Plugs.Notify.SmsAdapters.Handlers
{
    public interface INotifyHandler
    {
        Task<NotifyResp> NotifyMsg(NotifyTemplateConfig template, NotifyReq msg);
    }
}
