using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace TM.Module.Product;

/// <summary>
///  Sku 开放 WebApi 
/// </summary>
public class SkuController : BaseProductController, ISkuOpenService
{
    private static readonly ISkuOpenService _service = new SkuService();

    /// <summary>
    ///  查询Sku列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<TokenPageListResp<SkuMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }

    /// <summary>
    ///  通过id获取Sku详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<SkuMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///   通过id获取 有效可用的 Sku 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<SkuMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  设置Sku可用状态
    /// </summary>
    /// <param name="pass_token"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        return _service.SetUseable(pass_token, flag);
    }



    /// <summary>
    ///  添加Sku对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> Add([FromBody] AddSkuReq req)
    {
        return _service.Add(req);
    }
}
