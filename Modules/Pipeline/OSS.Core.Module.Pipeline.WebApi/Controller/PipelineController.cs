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
    ///  设置Pipe可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResp> SetUseable(long id, ushort flag)
    {
        await Task.Delay(2000);
        return await _service.SetUseable(id, flag);
    }



    /// <summary>
    ///  添加Pipe对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Add([FromBody] AddPipeReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    /// 搜索流水线信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageListResp<PipelineView>> SearchLines(SearchReq req)
    {
        return _service.SearchLines(req);
    }
}
