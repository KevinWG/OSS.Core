using OSS.Common;
using OSS.Common.Extension;
using OSS.Core.Context;

namespace OSS.Core.Domain
{
    /// <summary>
    ///  对象基类（含创建者Id和通用状态）
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class BaseOwnerAndStateMo<IdType> : BaseOwnerMo<IdType>, IDomainStatus
    {
        /// <inheritdoc />
        public CommonStatus status { get; set; }
    }

    /// <summary>
    ///  基础所有者实体
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class BaseOwnerMo<IdType> : BaseMo<IdType>
    {
        /// <summary>
        ///  【创建/归属】用户Id
        /// </summary>
        public long owner_uid { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long add_time { get; set; }

    }

    /// <summary>
    ///  基础所有者实体（租户）
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class BaseTenantOwnerMo<IdType> : BaseOwnerMo<IdType>, IDomainTenantId<long>
    {
        /// <summary>
        ///  租户
        /// </summary>
        public long tenant_id { get; set; }
    }

    /// <summary>
    ///  基础所有者实体（租户）
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class BaseTenantOwnerAndStateMo<IdType> : BaseTenantOwnerMo<IdType>
    {
        /// <summary>
        /// 状态信息
        /// </summary>
        public CommonStatus status { get; set; }
    }

    /// <summary>
    ///  基类扩展方法
    /// </summary>
    public static class BaseMoExtension
    {
        /// <summary>
        ///  从上下文中初始化基础信息
        /// </summary>
        /// <param name="t"></param>
        public static void FormatBaseByContext(this BaseOwnerMo<long> t)
        {
            if (t.id <= 0)
                t.id = NumHelper.SmallSnowNum();

            if (t.owner_uid <= 0)
            {
                if (CoreContext.User.IsAuthenticated)
                {
                    var userIdentity = CoreContext.User.Identity;
                    t.owner_uid = userIdentity.id.ToInt64();
                }
            }

            t.add_time = DateTime.Now.ToUtcSeconds();
        }

        /// <summary>
        ///  从上下文中初始化基础信息
        /// </summary>
        /// <param name="t"></param>
        public static void FormatBaseByContext(this BaseTenantOwnerMo<long> t)
        {
            ((BaseOwnerMo<long>)t).FormatBaseByContext();

            if (t.tenant_id > 0) return;

            if (CoreContext.Tenant.IsAuthenticated)
            {
                t.tenant_id = CoreContext.Tenant.Identity.id.ToInt64();
            }
        }
    }


}