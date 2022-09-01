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
    ///  启动流程
    /// </summary>
    /// <param name="flowId"></param>
    /// <returns></returns>
    Task<IResp> Start(long flowId);

    /// <summary>
    ///  主动投递节点处理结果
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Feed(FeedReq req);
}