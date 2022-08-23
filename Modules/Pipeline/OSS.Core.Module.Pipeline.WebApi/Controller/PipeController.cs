using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 开放 WebApi 
/// </summary>
public class PipeController : BasePipelineController, IPipeOpenService
{
    private static readonly IPipeOpenService _service = new PipeService();
    
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
    ///  添加管道
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp<long>> Add(AddPipeReq req)
    {
        return _service.Add(req);
    }
    
}
