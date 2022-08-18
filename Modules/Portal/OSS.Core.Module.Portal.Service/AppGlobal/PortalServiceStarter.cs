using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Module.Portal;

/// <summary>
/// 注册内部关联调用依赖
/// </summary>
public class PortalServiceStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        InsContainer<IAuthService>.Set<AuthService>();

        InsContainer<IAdminService>.Set<AdminService>();
        InsContainer<IUserService>.Set<UserService>();

        InsContainer<IFuncService>.Set<FuncService>();
        InsContainer<IRoleService>.Set<RoleService>();

        InsContainer<ISettingService>.Set<SettingService>();

        WechatOfficialQRHandler.StartSubscriber();
    }
}