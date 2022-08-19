using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

internal class PipeHttpClient : IPipeOpenService
{
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <returns></returns>
    public Task<PageListResp<PipeMo>> Search(SearchReq req)
    {
          return new PipelineRemoteReq("/Pipeline/Pipe/Search")
            .PostAsync<PageListResp<PipeMo>>(req);
    }

    /// <summary>
    ///  通过id获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IResp<PipeMo>> Get(long id)
    {
          return new PipelineRemoteReq($"/Pipeline/Pipe/Get?id={id}")
            .GetAsync<IResp<PipeMo>>();
    }

    
    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    public Task<IResp> SetUseable(long id, ushort flag)
    {
          return new PipelineRemoteReq($"/Pipeline/Pipe/SetUseable?id={id}&flag={flag}")
            .PostAsync<IResp>();
    }

    /// <summary>
    ///  添加对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<IResp> Add(AddPipeReq req)
    {
          return new PipelineRemoteReq($"/Pipeline/Pipe/Add")
            .PostAsync<IResp>(req);
    }
}

