using OSS.Common;
using OSS.Common.Extension;
using OSS.Core.Context;

namespace OSS.Core.Domain;

/// <summary>
///  基类扩展方法
/// </summary>
public static class BaseMoExtension
{

    #region BaseMo 相互转化

    /// <summary>
    ///  复制BaseMo属性信息
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void CopyBaseFrom<TId>(this BaseMo<TId> target, BaseMo<TId> source)
    {
        target.id       = source.id;
        target.add_time = source.add_time;
    }

    /// <summary>
    ///  复制 BaseTenantMo 属性信息
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void CopyBaseFrom<TId>(this BaseTenantMo<TId> target, BaseTenantMo<TId> source)
    {
        target.id       = source.id;
        target.add_time = source.add_time;
        target.tenant_id = source.tenant_id;
    }

    /// <summary>
    ///  复制 BaseOwnerMo 属性信息
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void CopyBaseFrom<TId>(this BaseOwnerMo<TId> target, BaseOwnerMo<TId> source)
    {
        target.id        = source.id;
        target.add_time  = source.add_time;
        target.owner_uid = source.owner_uid;
    }


    /// <summary>
    ///  复制 BaseTenantOwnerMo 属性信息
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void CopyBaseFrom<TId>(this BaseTenantOwnerMo<TId> target, BaseTenantOwnerMo<TId> source)
    {
        target.id       = source.id;
        target.add_time = source.add_time;

        target.tenant_id = source.tenant_id;
        target.owner_uid = source.owner_uid;
    }



    /// <summary>
    ///  复制 BaseTenantOwnerAndStateMo 属性信息
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void CopyBaseFrom<TId>(this BaseTenantOwnerAndStateMo<TId> target, BaseTenantOwnerAndStateMo<TId> source)
    {
        target.id       = source.id;
        target.add_time = source.add_time;

        target.tenant_id = source.tenant_id;
        target.owner_uid = source.owner_uid;

        target.status = source.status;
    }



    #endregion


    #region 初始化赋值

    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseMo<long> t)
    {
        t.add_time = DateTime.Now.ToUtcSeconds();
        if (t.id == 0)
            t.id = NumHelper.SmallSnowNum();
    }

    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseTenantMo<long> t)
    {
        ((BaseMo<long>)t).FormatBaseByContext();

        if (t.tenant_id == 0)
        {
            t.tenant_id = CoreContext.GetTenantLongIdSafely();
        }
    }


    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseOwnerMo<long> t)
    {
        ((BaseMo<long>)t).FormatBaseByContext();

        if (t.owner_uid == 0)
        {
            t.owner_uid = CoreContext.GetUserLongIdSafely();
        }
    }

    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseTenantOwnerMo<long> t)
    {
        ((BaseOwnerMo<long>)t).FormatBaseByContext();

        if (t.tenant_id == 0)
        {
            t.tenant_id = CoreContext.GetTenantLongIdSafely();
        }
    }


    //   ========  字符串类型  ========


    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseTenantMo<string> t)
    {
        t.add_time = DateTime.Now.ToUtcSeconds();
        if (t.tenant_id==0)
        {
            t.tenant_id = CoreContext.GetTenantLongIdSafely();
        }
    }


    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseOwnerMo<string> t)
    {
        t.add_time = DateTime.Now.ToUtcSeconds();
        if (t.owner_uid == 0)
        {
            t.owner_uid = CoreContext.GetUserLongIdSafely();
        }
    }

    /// <summary>
    ///  从上下文中初始化基础信息
    /// </summary>
    /// <param name="t"></param>
    public static void FormatBaseByContext(this BaseTenantOwnerMo<string> t)
    {
        ((BaseOwnerMo<string>)t).FormatBaseByContext();

        if (t.tenant_id == 0)
        {
            t.tenant_id = CoreContext.GetTenantLongIdSafely();
        }
    }


    #endregion



}