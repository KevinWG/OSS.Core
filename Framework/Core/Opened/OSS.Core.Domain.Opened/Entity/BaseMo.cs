namespace OSS.Core.Domain;

/// <summary>
///  （id + add_time）实体基类
/// </summary>
/// <typeparam name="IdType"></typeparam>
public abstract class BaseMo<IdType> : BaseIdMo<IdType>
{
    /// <summary>
    ///  创建时间
    /// </summary>
    public long add_time { get; set; }
}

/// <summary>
///  Id实体基类
/// </summary>
/// <typeparam name="IdType"></typeparam>
public abstract class BaseIdMo<IdType> : IDomainId<IdType>
{
    /// <inheritdoc />
    public IdType id { get; set; } = default!;
}