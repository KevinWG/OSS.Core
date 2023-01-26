using OSS.Common;
using OSS.Common.Resp;

namespace TM.Module.Product;

/// <summary>
///  Sku 领域对象开放接口
/// </summary>
public interface ISkuOpenService
{
    /// <summary>
    ///  Sku管理搜索列表（附带通过Id生成的PassToken）
    /// </summary>
    /// <returns></returns>
    Task<TokenPageListResp<SkuMo>> MSearch(SearchReq req);

    /// <summary>
    ///  通过id获取Sku详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<SkuMo>> Get(long id);

    /// <summary>
    ///  通过id获取有效可用状态的Sku详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<SkuMo>> GetUseable(long id);

    /// <summary>
    ///  设置Sku可用状态
    /// </summary>
    /// <param name="pass_token">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(string pass_token, ushort flag);

    /// <summary>
    ///  添加Sku对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddSkuReq req);
}
