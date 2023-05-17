namespace OSS.Core.Domain;

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