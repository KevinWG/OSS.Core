using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  Unit 领域对象开放接口
/// </summary>
public interface IUnitOpenService
{
    /// <summary>
    ///  所有单位列表
    /// </summary>
    /// <returns></returns>
    Task<ListResp<UnitView>> All();

    /// <summary>
    ///  添加单位对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddUnitReq req);
}
