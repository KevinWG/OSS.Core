using OSS.Common;
using OSS.Common.Extension;
using OSS.Core.Context;

namespace OSS.Core.Domain;

/// <summary>
///  基类扩展方法
/// </summary>
public static class BaseMoExtension
{
    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseMo<long> t)
    {
        t.add_time = DateTime.Now.ToUtcSeconds();
        if (t.id <= 0)
            t.id = NumHelper.SmallSnowNum();
    }

    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseTenantMo<long> t)
    {
        ((BaseMo<long>)t).FormatBaseByContext();

        if (t.tenant_id > 0) return;
        if (CoreContext.Tenant.IsAuthenticated)
        {
            t.tenant_id = CoreContext.Tenant.Identity.id.ToInt64();
        }
    }


    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseOwnerMo<long> t)
    {
        ((BaseMo<long>)t).FormatBaseByContext();
        
        if (t.owner_uid > 0 || !CoreContext.User.IsAuthenticated)
            return;
 
        var userIdentity = CoreContext.User.Identity;
        t.owner_uid = userIdentity.id.ToInt64();
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