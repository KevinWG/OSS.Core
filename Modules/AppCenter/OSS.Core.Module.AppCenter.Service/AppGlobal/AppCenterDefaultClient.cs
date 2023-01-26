using OSS.Common;

namespace OSS.Core.Module.AppCenter
{
    public class AppCenterDefaultClient : IAppCenterClient
    {
        public IOpenedAccessService AccessService { get; set; } = SingleInstance<AccessService>.Instance;
    }
}
