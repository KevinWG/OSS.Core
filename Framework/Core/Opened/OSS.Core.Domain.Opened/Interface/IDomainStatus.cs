namespace OSS.Core.Domain;

/// <inheritdoc />
public interface IDomainStatus: IDomainStatus<CommonStatus>
{
}

/// <summary>
///  对象状态
/// </summary>
public interface IDomainStatus<SType>
{
    /// <summary>
    /// 状态信息
    /// </summary>
    public SType status { get; set; }
}