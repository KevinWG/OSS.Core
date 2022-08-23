using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线 领域对象开放接口
/// </summary>
public interface IPipelineOpenService
{
    /// <summary>
    ///  设置流水线可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加流水线对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddPipeReq req);


    /// <summary>
    ///   搜索流水线信息
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<PipelineView>> SearchLines(SearchReq req);
}
