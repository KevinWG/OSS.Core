using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  Category 领域对象开放接口
/// </summary>
public interface ICategoryOpenService
{
    /// <summary>
    ///  查询Category列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<CategoryMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取Category详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<CategoryMo>> Get(long id);


    /// <summary>
    ///  通过id获取有效可用状态的分类信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<CategoryMo>> GetUseable(long id);

    /// <summary>
    ///  设置Category可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加Category对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddCategoryReq req);
}
