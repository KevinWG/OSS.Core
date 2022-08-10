using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal
{
    /// <inheritdoc />
    public class LocalUserAuthProvider : IUserAuthProvider
    {
        /// <inheritdoc />
        public async Task<IResp<UserIdentity>> GetIdentity()
        {
            return await SingleInstance<AuthService>.Instance.GetIdentity();
        }
    }
}
