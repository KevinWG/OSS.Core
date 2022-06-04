using OSS.Common.Resp;
using OSS.Core.Portal.Domain;

namespace OSS.Core.Portal.Shared.IService;

public interface ISharedUserService
{
    Task<Resp<UserBasicMo>> GetUserById(long userId);
}