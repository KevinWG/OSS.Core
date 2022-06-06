
using Microsoft.Extensions.DependencyInjection;

using OSS.Common;

using OSS.Core.Portal.Service.User;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Services.Basic.Portal;

namespace OSS.Core.Portal.Service;

public class PortalServiceStarter : ModuleStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        InsContainer<ISharedPortalService>.Set<PortalService>();
        InsContainer<ISharedUserService>.Set<UserService>();
    }
}

