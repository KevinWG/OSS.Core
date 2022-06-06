
using Microsoft.Extensions.DependencyInjection;

using OSS.Common;
using OSS.Core.Portal.Domain;

namespace OSS.Core.Portal.Repository
{
    public class PortalRepositoryStarter:ModuleStarter
    {
        public override void Start(IServiceCollection serviceCollection)
        {
            InsContainer<IUserInfoRep>.Set<UserInfoRep>();
            InsContainer<IAdminInfoRep>.Set<AdminInfoRep>();
        }
    }
}
