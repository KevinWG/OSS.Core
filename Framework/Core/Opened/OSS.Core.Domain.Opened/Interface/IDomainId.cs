namespace OSS.Core.Domain;

/// <inheritdoc />
public interface IDomainId: IDomainId<long>
{
}

/// <summary>
/// 对象id
/// </summary>
/// <typeparam name="IdType"></typeparam>
public interface IDomainId<IdType>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public IdType id { get; set; }
}