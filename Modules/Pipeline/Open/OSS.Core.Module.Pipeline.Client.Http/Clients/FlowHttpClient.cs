using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

internal class FlowHttpClient : IFlowOpenService
{
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <returns></returns>
    public Task<PageListResp<FlowMo>> Search(SearchReq req)
    {
          return new PipelineRemoteReq("/Pipeline/Flow/Search")
            .PostAsync<PageListResp<FlowMo>>(req);
    }

    /// <summary>
    ///  通过id获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IResp<FlowMo>> Get(long id)
    {
          return new PipelineRemoteReq($"/Pipeline/Flow/Get?id={id}")
            .GetAsync<IResp<FlowMo>>();
    }

    
    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    public Task<IResp> SetUseable(long id, ushort flag)
    {
          return new PipelineRemoteReq($"/Pipeline/Flow/SetUseable?id={id}&flag={flag}")
            .PostAsync<IResp>();
    }

    /// <summary>
    ///  添加对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<IResp> Add(AddFlowReq req)
    {
          return new PipelineRemoteReq($"/Pipeline/Flow/Add")
            .PostAsync<IResp>(req);
    }
}

