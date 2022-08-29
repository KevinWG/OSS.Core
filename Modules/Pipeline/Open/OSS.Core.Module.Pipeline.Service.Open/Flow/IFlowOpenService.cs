using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 领域对象开放接口
/// </summary>
public interface IFlowOpenService
{
    /// <summary>
    ///  查询Flow列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<FlowNodeMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取Flow详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<FlowNodeMo>> Get(long id);

    /// <summary>
    ///  
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Start(StartReq req);

    /// <summary>
    ///  流程节点执行输入
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Feed(FeedReq req);
    
}