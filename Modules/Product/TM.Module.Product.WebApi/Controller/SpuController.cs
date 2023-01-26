using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace TM.Module.Product;

/// <summary>
///  产品 开放 WebApi 
/// </summary>
public class SpuController : BaseProductController, ISpuOpenService
{
    private static readonly ISpuOpenService _service = new SpuService();

    /// <summary>
    ///  查询产品列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<TokenPageListResp<SpuMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }

    /// <summary>
    ///  通过id获取产品详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<SpuMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///   通过id获取 有效可用的 产品 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<SpuMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }
    
    /// <summary>
    ///  设置产品可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        return _service.SetUseable(pass_token, flag);
    }



    /// <summary>
    ///  添加产品对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> Add([FromBody] AddSpuReq req)
    {
        return _service.Add(req);
    }
}
