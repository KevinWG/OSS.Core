
using System;

namespace OSS.Core.Context
{
    /// <summary>
    ///   全局应用租户信息
    /// </summary>
    [Obsolete]
    public static class CoreTenantContext
    {
        /// <summary>
        ///   成员身份信息
        /// </summary>
        public static TenantIdentity Identity
            => CoreContext.Tenant.Identity;

        /// <summary>
        ///   设置用户信息
        /// </summary>
        /// <param name="info"></param>
        public static void SetIdentity(TenantIdentity info)
        {
            CoreContext.Tenant.Identity= info;
        }

        ///// <summary>
        ///// 获取租户扩展详情
        ///// </summary>
        ///// <typeparam name="TInfo"></typeparam>
        ///// <returns></returns>
        //public static TInfo GetTenantInfo<TInfo>()
        //    where TInfo : class => Identity?.TenantInfo as TInfo;
    }
}
