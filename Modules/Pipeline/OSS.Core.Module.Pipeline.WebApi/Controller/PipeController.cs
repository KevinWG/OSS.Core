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
    ///  添加管道
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> AddStart(AddPipeReq req)
    {
        return _service.AddStart(req);
    }


    /// <summary>
    ///  添加结束节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<LongResp> AddEnd(AddPipeReq req)
    {
        return _service.AddEnd(req);
    }



    /// <summary>
    ///  删除节点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Delete(long id)
    {
        return _service.Delete(id);
    }



}
