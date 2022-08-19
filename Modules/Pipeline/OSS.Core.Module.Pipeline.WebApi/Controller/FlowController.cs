using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

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
    ///  设置Flow可用状态
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
