using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线 领域对象开放接口
/// </summary>
public interface IPipelineOpenService
{

    /// <summary>
    ///  添加流水线对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddPipelineReq req);

    /// <summary>
    ///   搜索流水线信息
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<PipelineView>> SearchLines(SearchReq req);
}
