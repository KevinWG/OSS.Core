using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Module.Notify
{
    public class NotifyServiceStarter : AppStarter
    {
        public override void Start(IServiceCollection serviceCollection)
        {
            InsContainer<INotifyService>.Set<NotifyService>();
            InsContainer<ITemplateService>.Set<TemplateService>();
        }
    }
}
