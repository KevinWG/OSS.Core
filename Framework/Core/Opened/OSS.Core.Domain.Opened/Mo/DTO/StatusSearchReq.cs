using OSS.Common;

namespace OSS.Core.Domain;

/// <summary>
///  含状态搜索实体
/// </summary>
public class StatusSearchReq<SType> : SearchReq, IDomainStatus<SType>
{
    /// <inheritdoc />
    public SType status { get; set; }
}

/// <summary>
///  含状态搜索实体
/// </summary>
public class StatusSearchReq : StatusSearchReq<int>
{
}