using OSS.Common;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.Services.Basic.Portal.Common.Events;
using OSS.Core.Services.Basic.Portal.IProxies;

namespace OSS.Core.Services.Basic
{
    internal static class PortalInitiator
    {
        internal static void Initial()
        {
            RegisterServiceInstance();
            WechatQRAdapter.StartSubscriber();
        }

        private static void RegisterServiceInstance()
        {
            InsContainer<IPortalServiceProxy>.Set<PortalService>();
            InsContainer<IUserServiceProxy>.Set<UserService>();
        }
    }
}
