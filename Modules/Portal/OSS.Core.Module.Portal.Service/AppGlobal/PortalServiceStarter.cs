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
        InsContainer<IAuthCommonService>.Set<AuthService>();

        InsContainer<IAdminCommonService>.Set<AdminService>();
        InsContainer<IUserCommonService>.Set<UserService>();
        InsContainer<ISocialUserCommonService>.Set<SocialUserService>();

        InsContainer<IFuncCommonService>.Set<FuncService>();
        InsContainer<IRoleCommonService>.Set<RoleService>();

        InsContainer<ISettingCommonService>.Set<SettingService>();

        WechatOfficialQRHandler.StartSubscriber();
    }
}