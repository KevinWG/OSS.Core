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
            ///   请求租户授权信息
            /// </summary>
            public static TenantIdentity Identity
            {
                get { return ContextHelper.GetContext()?.TenantIdentity; }
                set { ContextHelper.SetTenantIdentity(value); }
            }
        }

    }
}
