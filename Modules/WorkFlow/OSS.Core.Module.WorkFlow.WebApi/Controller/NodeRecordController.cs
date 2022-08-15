using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.WorkFlow;

/// <summary>
/// 实体对外WebApi
/// </summary>
public class NodeRecordController : BaseWorkFlowController, IOpenedNodeRecordService
{
    public static readonly NodeRecordService _service = new();

    /// <inheritdoc />
    [HttpPost]
    public Task<PageListResp<NodeRecordMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<NodeRecordMo>> Get(long id)
    {
        return _service.Get(id);
    }
    
    /// <summary>
    ///  设置可用状态
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



    /// <inheritdoc />
    [HttpPost]
    public Task<IResp> Add([FromBody] AddNodeRecordReq req)
    {
        return _service.Add(req);
    }
}
