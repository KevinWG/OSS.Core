using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 服务
/// </summary>
public class FlowService : IFlowOpenService
{
    private static readonly FlowRep _FlowRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<FlowMo>> Search(SearchReq req)
    {
        return new PageListResp<FlowMo>(await _FlowRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<FlowMo>> Get(long id) => _FlowRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _FlowRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddFlowReq req)
    {
        var mo = req.MapToFlowMo();
        mo.FormatBaseByContext();

        await _FlowRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
