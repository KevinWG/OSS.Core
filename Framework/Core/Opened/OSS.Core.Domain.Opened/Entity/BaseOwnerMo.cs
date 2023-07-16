namespace OSS.Core.Domain;

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
}

/// <summary>
///  对象基类（含创建者Id和通用状态）
/// </summary>
/// <typeparam name="IdType"></typeparam>
public class BaseOwnerAndStateMo<IdType> : BaseOwnerMo<IdType>, IDomainStatus
{
    /// <inheritdoc />
    public CommonStatus status { get; set; }
}