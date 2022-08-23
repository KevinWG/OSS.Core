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
    ///     （同一流水线仅显示一个版本
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<PipelineView>> SearchLines(SearchReq req);

    /// <summary>
    ///   通过metaId获取所有版本流水线列表
    /// </summary>
    /// <param name="metaId"></param>
    /// <returns></returns>
    Task<List<PipelineView>> GetVersions(long metaId);
}
