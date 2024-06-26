﻿namespace OSS.Core.Domain;


/// <summary>
///  租户基类 (租户id + BaseMo)
/// </summary>
/// <typeparam name="IdType"></typeparam>
public class BaseTenantMo<IdType> : BaseMo<IdType>, IDomainTenantId<long>
{
    /// <summary>
    ///  租户
    /// </summary>
    public long tenant_id { get; set; }
}


/// <summary>
///  所有者实体（租户+用户）基类
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
///  所有者实体（租户+用户+状态）基类
/// </summary>
/// <typeparam name="IdType"></typeparam>
public class BaseTenantOwnerAndStateMo<IdType> : BaseTenantOwnerMo<IdType>
{
    /// <summary>
    /// 状态信息
    /// </summary>
    public CommonStatus status { get; set; }
}