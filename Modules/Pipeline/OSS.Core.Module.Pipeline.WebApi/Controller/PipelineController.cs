using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线 开放 WebApi 
/// </summary>
public class PipelineController : BasePipelineController, IPipelineOpenService
{
    private static readonly IPipelineOpenService _service = new PipelineService();
    
    /// <summary>
    ///  添加Pipe对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Add([FromBody] AddPipelineReq req)
    {
        return _service.Add(req);
    }
    
    /// <summary>
    /// 搜索流水线信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<PipelineView>> SearchLines(SearchReq req)
    {
        return _service.SearchLines(req);
    }

    /// <summary>
    ///   通过metaId获取所有版本流水线列表
    /// </summary>
    /// <param name="meta_id">流水线MetaId</param>
    /// <returns></returns>
    public Task<List<PipelineView>> GetVersions(long meta_id)
    {
        return _service.GetVersions(meta_id);
    }



}
