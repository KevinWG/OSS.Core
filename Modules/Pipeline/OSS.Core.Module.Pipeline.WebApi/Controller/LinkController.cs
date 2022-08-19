using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Link 开放 WebApi 
/// </summary>
public class LinkController : BasePipelineController, ILinkOpenService
{
    private static readonly ILinkOpenService _service = new LinkService();

    /// <summary>
    ///  查询Link列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<LinkMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    ///  通过id获取Link详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<LinkMo>> Get(long id)
    {
        return _service.Get(id);
    }
    
    /// <summary>
    ///  设置Link可用状态
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
    ///  添加Link对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Add([FromBody] AddLinkReq req)
    {
        return _service.Add(req);
    }
}
