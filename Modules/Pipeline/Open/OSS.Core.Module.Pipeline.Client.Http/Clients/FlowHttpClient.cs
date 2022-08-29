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
    public Task<PageListResp<FlowNodeMo>> Search(SearchReq req)
    {
          return new PipelineRemoteReq("/Pipeline/Flow/Search")
            .PostAsync<PageListResp<FlowNodeMo>>(req);
    }

    /// <summary>
    ///  通过id获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IResp<FlowNodeMo>> Get(long id)
    {
          return new PipelineRemoteReq($"/Pipeline/Flow/Get?id={id}")
            .GetAsync<IResp<FlowNodeMo>>();
    }

    public Task<IResp> Start(StartReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Flow/Start")
            .PostAsync<IResp>(req);
    }

    public Task<IResp> Feed(FeedReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Flow/Feed")
            .PostAsync<IResp>(req);
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

}

