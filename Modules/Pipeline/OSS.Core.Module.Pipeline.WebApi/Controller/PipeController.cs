using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 开放 WebApi 
/// </summary>
public class PipeController : BasePipelineController, IPipeOpenService
{
    private static readonly IPipeOpenService _service = new PipeService();

    #region 起始节点

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
    [HttpPost]
    public Task<LongResp> AddEnd(AddPipeReq req)
    {
        return _service.AddEnd(req);
    }

    #endregion

    #region 审核节点管理

    /// <summary>
    ///  添加审核节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> AddAudit(AddPipeReq req)
    {
        return _service.AddAudit(req);
    }


    /// <summary>
    ///  设置审核节点执行扩展信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ext"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> SetAuditExe(long id, [FromBody] AuditExt ext)
    {
        return _service.SetAuditExe(id, ext);
    }

    #endregion

    #region 子流水线节点管理

    /// <summary>
    ///  添加审核节点
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> AddSubPipeline(AddPipeReq req)
    {
        return _service.AddSubPipeline(req);
    }


    /// <summary>
    ///  设置子流水线执行扩展信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ext"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> SetSubPipelineExe(long id, [FromBody] SubPipeLineExt ext)
    {
        return _service.SetSubPipelineExe(id, ext);
    }

    #endregion

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
