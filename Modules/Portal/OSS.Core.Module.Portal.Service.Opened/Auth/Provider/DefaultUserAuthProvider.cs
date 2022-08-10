using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal
{
    /// <inheritdoc />
    public class DefaultUserAuthProvider : IUserAuthProvider
    {
        /// <inheritdoc />
        public Task<IResp<UserIdentity>> GetIdentity()
        {
            return InsContainer<IPortalClient>.Instance.Auth.GetIdentity();
        }
    }
}
