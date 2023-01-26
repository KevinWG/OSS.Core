using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
/// 单位信息
/// </summary>
public class UnitView : BaseMo<long>
{
    /// <summary>
    /// 单位名称
    /// </summary>
    public string name { get; set; } = string.Empty;
}