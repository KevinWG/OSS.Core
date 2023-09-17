using OSS.Common;

namespace OSS.Core.Domain;

/// <summary>
///  含状态搜索实体
/// </summary>
public class StatusSearchReq<SType> : SearchReq, IDomainStatus<SType>
    where SType : struct, IConvertible
{
    /// <summary>
    ///  搜索的状态信值
    ///    <para> 一般搜索时，判断传入的值: </para>
    ///    <para>     如果以 9 结尾，查询 大于 传入的值 的状态 </para>
    ///    <para>     如果以 1 结尾，查询 小于 传入的值 的状态 </para>
    ///    <para>     其他，查询 等于 传入的值 的状态         </para>
    /// </summary>
    public SType status { get; set; } 
}

/// <summary>
///  含状态搜索实体
/// </summary>
public class StatusSearchReq : StatusSearchReq<int>
{
    /// <inheritdoc />
    public StatusSearchReq()
    {
        // 默认值
        status = -999;
    }
}