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
    Task<PageListResp<FlowMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取Flow详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<FlowMo>> Get(long id);

    /// <summary>
    ///  添加Flow对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddFlowReq req);
}
