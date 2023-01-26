using OSS.Common;
using OSS.Common.Resp;

namespace TM.Module.Product;

/// <summary>
///  ProductCategory 领域对象开放接口
/// </summary>
public interface ISpuCategoryOpenService
{
    /// <summary>
    ///  获取当前 产品分类 信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<SpuCategoryMo>> Get(long id);

    ///// <summary>
    /////  搜索 产品分类 列表
    ///// </summary>
    ///// <returns></returns>
    //Task<PageListResp<SpuCategoryMo>> MSearch(SearchReq req);

    /// <summary>
    ///  产品所有分类
    /// </summary>
    /// <returns></returns>
    Task<ListResp<SpuCategoryTreeItem>> AllCategories();

    ///// <summary>
    /////  通过id获取有效可用状态的  产品分类 详情
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //Task<IResp<SpuCategoryMo>> GetUseable(long id);

    /// <summary>
    /// 添加 产品分类
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddSpuCategoryReq req);

    /// <summary>
    ///  设置 产品分类 可用状态
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(long id, ushort flag);

    /// <summary>
    /// 编辑 产品分类
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> UpdateName(long id, UpdateSCNameReq req);

    /// <summary>
    /// 编辑 产品分类 排序值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order">排序值</param>
    /// <returns></returns>
    Task<Resp> UpdateOrder(long id, int order);
}