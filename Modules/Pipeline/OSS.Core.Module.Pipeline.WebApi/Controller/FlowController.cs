using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 开放 WebApi 
/// </summary>
public class FlowController : BasePipelineController, IFlowOpenService
{
    private static readonly IFlowOpenService _service = new FlowService();

    /// <summary>
    ///  查询Flow列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<FlowMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    ///  通过id获取Flow详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<FlowMo>> Get(long id)
    {
        return _service.Get(id);
    }
    

    /// <summary>
    ///  添加Flow对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Add([FromBody] AddFlowReq req)
    {
        return _service.Add(req);
    }
}
