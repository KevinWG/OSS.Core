using OSS.Common.Extension;

namespace OSS.Core.Context
{
    using Provider =  OSS.Common.ServiceProvider;

    /// <summary>
    /// OSSCore 核心上下文信息
    /// </summary>
    public static partial class CoreContext
    {
        /// <summary>
        ///  全局ServiceProvider
        /// </summary>
        public static IServiceProvider? ServiceProvider
        {
            get => Provider.Provider; 
            set => Provider.Provider = value;
        }

        /// <summary>
        ///  初始化上下文容器
        /// </summary>
        public static void InitialContextContainer()
        {
            ContextHelper.GetContext();
        }

        /// <summary>
        ///  获取用户Id（long类型）
        /// </summary>
        /// <returns></returns>
        public static long GetUserLongId()
        {
            return User.Identity.id.ToInt64();
        }

        /// <summary>
        ///  获取租户Id（long类型）
        /// </summary>
        /// <returns></returns>
        public static long GetTenantLongId()
        {
            return Tenant.Identity.id.ToInt64();
        }

        /// <summary>
        ///  获取用户Id（long类型）
        /// </summary>
        /// <returns></returns>
        public static long GetUserLongIdSafely()
        {
            return User.IsAuthenticated? User.Identity.id.ToInt64():0;
        }

        /// <summary>
        ///  获取租户Id（long类型）
        /// </summary>
        /// <returns></returns>
        public static long GetTenantLongIdSafely()
        {
            return Tenant.IsAuthenticated? Tenant.Identity.id.ToInt64():0;
        }
    }
    
}
