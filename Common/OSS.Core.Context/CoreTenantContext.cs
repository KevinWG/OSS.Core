using OSS.Core.Context.Helper;
using OSS.Core.Context.Mos;

namespace OSS.Core.Context
{
    /// <summary>
    ///   全局应用租户信息
    /// </summary>
    public static class CoreTenantContext
    {
        /// <summary>
        ///   成员身份信息
        /// </summary>
        public static TenantIdentity Identity
            => ContextHelper.GetContext()?.TenantIdentity;

        /// <summary>
        ///   设置用户信息
        /// </summary>
        /// <param name="info"></param>
        public static void SetIdentity(TenantIdentity info)
        {
            ContextHelper.SetTenantIdentity(info);
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
