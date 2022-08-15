using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Module.Portal;

/// <summary>
///  门户仓储层启动注册配置
/// </summary>
public class PortalRepositoryStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        InsContainer<IUserInfoRep>.Set<UserInfoRep>();
        InsContainer<IUserInfoRep>.Set<UserInfoRep>();
        InsContainer<ISocialUserRep>.Set<SocialUserRep>();

        InsContainer<IAdminInfoRep>.Set<AdminInfoRep>();

        // 权限
        InsContainer<IFuncRep>.Set<FuncRep>();
        InsContainer<IRoleRep>.Set<RoleRep>();
        InsContainer<IRoleFuncRep>.Set<RoleFuncRep>();
        InsContainer<IRoleUserRep>.Set<RoleUserRep>();
    }
}