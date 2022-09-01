using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

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
    /// <param name="id">业务流Id</param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Start(long id)
    {
        return _service.Start(id);
    }

    /// <summary>
    ///  主动投递节点处理结果
    ///     （服务端内部应用调用
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [AppMeta(AppAuthMode.AppSign)]
    public Task<IResp> Feed(FeedReq req)
    {
        return _service.Feed(req);
    }
}
