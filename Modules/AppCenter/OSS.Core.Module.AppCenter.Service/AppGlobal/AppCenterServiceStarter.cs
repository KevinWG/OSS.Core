using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Module.AppCenter
{
    public class AppCenterServiceStarter : AppStarter
    {
        public override void Start(IServiceCollection serviceCollection)
        {
            InsContainer<IAccessService>.Set<AccessService>();
        }
    }
}
