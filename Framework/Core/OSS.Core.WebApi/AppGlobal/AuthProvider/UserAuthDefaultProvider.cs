using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Module.Portal;

namespace OSS.Core
{
    /// <inheritdoc />
    public class UserAuthDefaultProvider : IUserAuthProvider
    {
        /// <inheritdoc />
        public Task<IResp<UserIdentity>> GetIdentity()
        {
            return InsContainer<IPortalClient>.Instance.Auth.GetIdentity();
        }
    }
}
