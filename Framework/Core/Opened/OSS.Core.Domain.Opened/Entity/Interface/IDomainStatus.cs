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
    ///    <para> 定义枚举值时, 为了方便通用搜索和后续扩展 </para>
    ///    <para> 一般状态取值的范围： 10 的倍数</para>
    ///    <para> 搜索时，判断传入的值:                    </para>
    ///    <para>     如果以 9 结尾，查询 大于 传入的值 的状态   </para>
    ///    <para>     如果以 1 结尾，查询 小于 传入的值 的状态   </para>
    ///    <para>     其他，查询 等于 传入的值 的状态           </para>
    /// </summary>
    public SType status { get; set; }
}