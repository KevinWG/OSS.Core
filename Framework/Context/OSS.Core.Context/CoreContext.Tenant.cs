using OSS.Common.Resp;

namespace OSS.Core.Context
{
    public static partial class CoreContext
    {
        /// <summary>
        ///   全局应用租户信息
        /// </summary>
        public static class Tenant
        {
            /// <summary>
            /// 是否已经授权认证
            /// </summary>
            public static bool IsAuthenticated => ContextHelper.GetContext().TenantIdentity != null;
            
            /// <summary>
            ///   请求租户授权信息
            /// </summary>
            public static TenantIdentity Identity
            {
                get
                {
                    var identity = ContextHelper.GetContext().TenantIdentity;
                    if (identity == null)
                    {
                        throw new RespException(SysRespCodes.NotImplement, $"未能获取有效租户信息!");
                    }
                    return identity;
                }

                set => ContextHelper.SetTenantIdentity(value);
            }
        }

    }
}
