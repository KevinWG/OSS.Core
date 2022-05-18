using OSS.Common;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Portal.Shared.IService.Portal;
using OSS.Core.Services.Basic.Portal;

namespace OSS.Core.Portal.Service
{
    internal static class PortalInitiator
    {
        internal static void Initial()
        {
            RegisterServiceInstance();
        }

        private static void RegisterServiceInstance()
        {
            InsContainer<IPortalService>.Set<PortalService>();
            InsContainer<IUserService>.Set<UserService>();
        }
    }
}
