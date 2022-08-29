using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 开放 WebApi 
/// </summary>
public class FlowNodeController : BasePipelineController, IFlowOpenService
{
    private static readonly IFlowOpenService _service = new FlowService();

    /// <summary>
    ///  查询Flow列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<FlowNodeMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    ///  通过id获取Flow详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<FlowNodeMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///  流程启动
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<IResp> Start(StartReq req)
    {
        return _service.Start(req);
    }

    /// <summary>
    ///  流程节点执行输入
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<IResp> Feed(FeedReq req)
    {
        return _service.Feed(req);
    }
}
