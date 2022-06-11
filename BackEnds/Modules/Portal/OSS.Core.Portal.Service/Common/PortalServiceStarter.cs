using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Portal.Service;

/// <summary>
///   门户服务端启动注册
/// </summary>
public class PortalServiceStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        InsContainer<IAuthService>.Set<AuthService>();
        InsContainer<IUserService>.Set<UserService>();
        InsContainer<IAdminService>.Set<AdminService>();
    }
}
 