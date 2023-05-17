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
    ///     使用枚举定义时，必须是 10 的倍数
    ///     搜索时，判断传入的值:
    ///         如果以 9 结尾，查询 大于 传入的值 的状态
    ///         如果以 1 结尾，查询 小于 传入的值 的状态
    ///         其他，查询 等于 传入的值 的状态
    /// </summary>
    public SType status { get; set; }
}