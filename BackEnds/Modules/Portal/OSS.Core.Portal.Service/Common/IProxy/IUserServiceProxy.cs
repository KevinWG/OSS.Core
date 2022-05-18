using OSS.Common.Resp;
using System.Threading.Tasks;
using OSS.Core.Reps.Basic.Portal.Mos;

namespace OSS.Core.Services.Basic.Portal.IProxies
{
    public interface IUserServiceProxy
    {
        Task<Resp<UserBasicMo>> GetUserById(long userId);
    }
}
