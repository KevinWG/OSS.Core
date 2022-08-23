using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

internal class PipeHttpClient : IPipeOpenService
{
    
    
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


    public Task<PageListResp<PipelineView>> SearchMetas()
    {
        throw new NotImplementedException();
    }

    public Task<IResp<PipelineDetailView>>                          GetPipelineDetail(long id)
    {
        throw new NotImplementedException();
    }
}

